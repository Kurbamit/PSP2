import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import {Order} from "./Orders.tsx";
import { getOrderStatusString, getYesNoString } from "../../../assets/Utils/utils.ts";
import SelectDropdown from "../../Base/SelectDropdown.tsx";

interface Item {
    itemId: number;
    name: string;
    cost: number;
    tax: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: number | null;
}

interface FullOrder {
    order: Order;
    items: Array<Item>;
}

const OrderDetail: React.FC = () => {
    const [order, setOrder] = useState<FullOrder | null>(null);
    const [editedItem, setEditedItem] = useState<FullOrder | null>(null); // Store edited item
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [selectedItemId, setSelectedItemId] = useState<number | null>(null);
    const [showModal, setShowModal] = useState(false);

    const fetchItem = async () => {
        try {
            const response = await axios.get(`http://localhost:5114/api/orders/${id}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            setOrder(response.data);
            setEditedItem(response.data);
        } catch (error) {
            console.error(ScriptResources.ErrorFetchingItems, error);
        }
    };

    useEffect(() => {
        fetchItem();
    }, [id, token]);

    const handleAddItem = async () => {
        console.log('Adding item:', selectedItemId);
        console.log('Order ID:', id);
        if (selectedItemId && id) {
            try {
                await axios.put(
                    `http://localhost:5114/api/orders/${id}/items`,
                    { orderId: Number(id), itemId: selectedItemId, count: 1 },
                    { headers: { Authorization: `Bearer ${token}` } }
                );
                setShowModal(false); // Close the modal after adding the item
                setSelectedItemId(null); // Reset the selected item
                fetchItem(); // Refresh the order details
            } catch (error) {
                console.error(ScriptResources.ErrorAddingItem, error);
            }
        }
    };

    const handleDelete = async (itemId: number) => {
        if (id) {
            try {
                await axios.delete(`http://localhost:5114/api/orders/${id}/items`, {
                    headers: { Authorization: `Bearer ${token}` },
                    data: { orderId: Number(id), itemId, count: 1 }, // Send the body with data
                });
                fetchItem(); // Refresh the order details after deletion
            } catch (error) {
                console.error(ScriptResources.ErrorDeletingItem, error);
            }
        }
    };
    
    const handleBackToList = () => {
        navigate('/orders');
    };

    return (
        <div className="container-fluid">
            <h2 className="mb-4">{ScriptResources.Order}</h2>
            {editedItem ? (
                <div className="card w-100">
                    <div className="card-body">
                        <h5 className="card-title">{ScriptResources.OrderInformation}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>{ScriptResources.OrderId}</strong> {order?.order.orderId}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Status}</strong>
                                <p>{getOrderStatusString(editedItem.order.status)}</p>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ReceiveTime}</strong>
                                <p>{new Date(editedItem.order.receiveTime).toLocaleString()}</p>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.DiscountPercentage}</strong>
                                <p>{editedItem.order.discountPercentage ? editedItem.order.discountPercentage : '-'}</p>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.DiscountFixed}</strong>
                                <p>{editedItem.order.discountFixed ? editedItem.order.discountFixed : '-'}</p>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Refunded}</strong>
                                <p>{getYesNoString(editedItem.order.refunded)}</p>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.CreatedBy}</strong>
                                <p>{editedItem.order.createdByEmployeeName}</p>
                            </li>
                        </ul>
                    </div>
                </div>
            ) : (
                <div>{ScriptResources.Loading}</div>
            )}

            
            
            {/* Render Items Table */}
            {editedItem && editedItem.items.length > 0 && (
                <div className="mt-4">
                    <h3>{ScriptResources.Items}</h3>
                    <div className="d-flex justify-content-between align-items-center mb-2">
                        <button className="btn btn-primary" onClick={() => setShowModal(true)}>
                            {ScriptResources.AddItem}
                        </button>
                    </div>
                    <table className="table table-striped table-bordered">
                        <thead>
                        <tr>
                            <th>{ScriptResources.ItemId}</th>
                            <th>{ScriptResources.Name}</th>
                            <th>{ScriptResources.Cost}</th>
                            <th>{ScriptResources.Tax}</th>
                            <th>{ScriptResources.AlcoholicBeverage}</th>
                            <th>{ScriptResources.ReceiveTime}</th>
                            <th>{ScriptResources.Storage}</th>
                            <th>{ScriptResources.Actions}</th>
                        </tr>
                        </thead>
                        <tbody>
                        {editedItem.items.map((item) => (
                            <tr key={item.itemId}>
                                <td>{item.itemId}</td>
                                <td>{item.name}</td>
                                <td>{item.cost}</td>
                                <td>{item.tax}</td>
                                <td>{getYesNoString(item.alcoholicBeverage)}</td>
                                <td>{item.receiveTime}</td>
                                <td>{item.storage ?? ScriptResources.NotAvailable}</td>
                                <td>
                                    <span
                                        className="material-icons"
                                        style={{cursor: 'pointer', marginRight: '10px'}}
                                        onClick={() => handleDelete(item.itemId)}
                                    >
                                        delete
                                </span>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            )}

            <div className="mt-3">
                {/*TODO: Checkout and Close order buttons*/}
                <div className="d-flex mb-3">
                    <button className="btn btn-primary me-2" onClick={() => setShowModal(true)}>
                        {ScriptResources.Checkout}
                    </button>
                    <button className="btn btn-primary" onClick={() => setShowModal(true)}>
                        {ScriptResources.Close}
                    </button>
                </div>
                
                <button className="btn btn-secondary" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
            </div>

            {/* Modal */}
            {showModal && (
                <div className="modal show d-block" style={{backgroundColor: "rgba(0,0,0,0.5)"}}>
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{ScriptResources.AddNewItem}</h5>
                                <button className="btn-close" onClick={() => setShowModal(false)}></button>
                            </div>
                            <div className="modal-body">
                                <SelectDropdown
                                    endpoint="/AllItems"
                                    onSelect={(item) => {
                                        if (item) {
                                            setSelectedItemId(item.id);
                                        }
                                    }}
                                />
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => setShowModal(false)}>
                                    {ScriptResources.Cancel}
                                </button>
                                <button className="btn btn-primary" onClick={handleAddItem}>
                                    {ScriptResources.Add}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default OrderDetail;
