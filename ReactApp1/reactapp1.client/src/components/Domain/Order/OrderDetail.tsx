import React, {useEffect, useState} from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import {useNavigate, useParams} from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import {Order} from "./Orders.tsx";
import {getOrderStatusString, getYesNoString} from "../../../assets/Utils/utils.ts";
import SelectDropdown from "../../Base/SelectDropdown.tsx";
import {OrderStatusEnum} from "../../../assets/Models/FrontendModels.ts";
import { Form } from 'react-bootstrap';

interface Item {
    itemId: number;
    name: string;
    cost: number;
    tax: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: number | null;
    count: number | null;
}

interface FullOrder {
    order: Order;
    items: Array<Item>;
    payments: Array<Payment>;
}

interface Payment {
    type: number;
    amount: number;
}

enum PaymentMethodEnum {
    Cash = 1,
    GiftCard = 2,
    Card = 3,
}

const OrderDetail: React.FC = () => {
    const [order, setOrder] = useState<FullOrder | null>(null);
    const [editedItem, setEditedItem] = useState<FullOrder | null>(null); // Store edited item
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [selectedItemId, setSelectedItemId] = useState<number | null>(null);
    const [showItemModal, setShowItemModal] = useState(false);
    const [showPaymentModal, setShowPaymentModal] = useState(false);
    const [count, setCount] = useState(1);
    const [paymentValue, setPaymentValue] = useState<number>(0);
    const [paymentType, setPaymentType] = useState<PaymentMethodEnum>(PaymentMethodEnum.Cash);

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
                    { orderId: Number(id), itemId: selectedItemId, count: count },
                    { headers: { Authorization: `Bearer ${token}` } }
                );
                setShowItemModal(false); // Close the modal after adding the item
                setSelectedItemId(null); // Reset the selected item
                fetchItem(); // Refresh the order details
                setCount(1);
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
    
    const handleClose = async () => {
        if (id) {
            try {
                await axios.put(
                    `http://localhost:5114/api/orders/${id}/close`,
                    { orderId: Number(id) },
                    { headers: { Authorization: `Bearer ${token}` } }
                );
                fetchItem();
            } catch (error) {
                console.error(ScriptResources.ErrorClosingOrder, error);
            }
        }
    }

    const handleCancel = async () => {
        if (id) {
            try {
                await axios.put(
                    `http://localhost:5114/api/orders/${id}/cancel`,
                    { orderId: Number(id) },
                    { headers: { Authorization: `Bearer ${token}` } }
                );
                navigate('/orders');
            } catch (error) {
                console.error(ScriptResources.ErrorCancellingOrder, error);
            }
        }
    }

    const handlePaymentTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setPaymentType(Number(e.target.value) as PaymentMethodEnum);
    };

    const handlePayPayment = async () => {
        if (id && paymentValue > 0) {
            try {
                await axios.put(
                    `http://localhost:5114/api/orders/${id}/pay`,
                    {
                        orderId: Number(id),
                        type: paymentType,
                        value: paymentValue,
                        giftCardId: 0,
                    },
                    { headers: { Authorization: `Bearer ${token}` } }
                );
 
                setShowPaymentModal(false);
                setPaymentType(PaymentMethodEnum.Cash);
                setPaymentValue(0);
                fetchItem(); 
            } catch (error) {
                console.error(ScriptResources.ErrorPayment, error);
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
                            <li className="list-group-item">
                                <strong>{ScriptResources.TotalPrice}</strong>
                                <p>{editedItem.order.totalPrice ? editedItem.order.totalPrice + ' ' + ScriptResources.Eur : '-'}</p>
                            </li>
                        </ul>
                    </div>
                </div>
            ) : (
                <div>{ScriptResources.Loading}</div>
            )}


            {/* Render Items Table */}
            {editedItem && (
                <div className="mt-4">
                    <h3>{ScriptResources.Items}</h3>

                    {/* Show AddItem button */}
                    {order?.order.status === OrderStatusEnum.Open && (
                        <div className="d-flex justify-content-between align-items-center mb-2">
                            <button className="btn btn-primary" onClick={() => setShowItemModal(true)}>
                                {ScriptResources.AddItem}
                            </button>
                        </div>
                    )}

                    {/* Render table only if there are items */}
                    {editedItem.items.length > 0 ? (
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
                                <th>{ScriptResources.Count}</th>
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
                                    <td>{item.count ?? ' '}</td>
                                    <td>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginRight: '10px' }}
                                    onClick={() => handleDelete(item.itemId)}
                                >
                                    delete
                                </span>
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    ) : (
                        // Show empty table message when there are no items
                        <p>{ScriptResources.NoItems}</p>
                    )}
                </div>
            )}

            <div className="mt-3">
                {order?.order.status === OrderStatusEnum.Open && (
                    <div className="d-flex mb-3">
                        <button className="btn btn-primary me-2" onClick={() => handleClose()}>
                            {ScriptResources.Checkout}
                        </button>
                        <button className="btn btn-primary" onClick={() => handleCancel()}>
                            {ScriptResources.Cancel}
                        </button>
                    </div>
                )}
                
                <button className="btn btn-secondary" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
            </div>

            {/* Modal */}
            {showItemModal && (
                <div className="modal show d-block" style={{backgroundColor: "rgba(0,0,0,0.5)"}}>
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{ScriptResources.AddNewItem}</h5>
                                <button className="btn-close" onClick={() => setShowItemModal(false)}></button>
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
                                <Form.Group className="mb-3" controlId="item-count">
                                    <Form.Label>{ScriptResources.SelectCount}</Form.Label>
                                    <Form.Control
                                        type="number"
                                        value={count}
                                        onChange={(e) => setCount(Number(e.target.value))}
                                        min="1"
                                    />
                                </Form.Group>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => {setShowItemModal(false); setCount(1)}}>
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

            {order?.order.status === OrderStatusEnum.Closed && (
                <div className="mt-4">
                    <h3>{ScriptResources.Payments}</h3>
                    {order.payments.length > 0 ? (
                        <table className="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>{ScriptResources.PaymentType}</th>
                                    <th>{ScriptResources.Amount}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {order.payments.map((payment, index) => (
                                    <tr key={index}>
                                        <td>{payment.type}</td>
                                        <td>{payment.value} {ScriptResources.Eur}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    ) : (
                        <p>{ScriptResources.NoPayments}</p>
                    )}
                    <button className="btn btn-primary mt-3" onClick={() => setShowPaymentModal(true)}>
                        {ScriptResources.AddPayment}
                    </button>
                </div>
            )}

            {showPaymentModal && (
            <div className="modal show d-block" style={{ backgroundColor: "rgba(0,0,0,0.5)" }}>
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{ScriptResources.AddPayment}</h5>
                            <button className="btn-close" onClick={() => setShowPaymentModal(false)}></button>
                        </div>
                        <div className="modal-body">
                            <Form.Group className="mb-3" controlId="payment-method">
                                    <Form.Label>{ScriptResources.PaymentType}</Form.Label>
                                    <Form.Select value={paymentType} onChange={handlePaymentTypeChange}>
                                        <option value={PaymentMethodEnum.Cash}>{ScriptResources.Cash}</option>
                                        <option value={PaymentMethodEnum.GiftCard}>{ScriptResources.GiftCard}</option>
                                        <option value={PaymentMethodEnum.Card}>{ScriptResources.Card}</option>
                                    </Form.Select>
                            </Form.Group>
                            <Form.Group className="mb-3" controlId="payment-amount">
                                <Form.Label>{ScriptResources.Amount}</Form.Label>
                                <Form.Control
                                    type="number"
                                    value={paymentValue}
                                    onChange={(e) => setPaymentValue(Number(e.target.value))}
                                    min="0"
                                />
                            </Form.Group>
                        </div>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" onClick={() => setShowPaymentModal(false)}>
                                {ScriptResources.Cancel}
                            </button>
                            <button className="btn btn-primary" onClick={handlePayPayment}>
                                {ScriptResources.Pay}
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
