// src/components/Domain/Order/Orders.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";
import { OrderStatusEnum} from "../../../assets/Models/FrontendModels.ts";
import { getOrderStatusString } from "../../../assets/Utils/utils.ts";

export interface Order {
    orderId: number;
    status: OrderStatusEnum;
    createdByEmployeeId: number;
    createdByEmployeeName: string;
    receiveTime: string;
    discountPercentage: number | null;
    discountFixed: number | null;
    tipPercentage: number | undefined;
    tipFixed: number | undefined;
    paymentId: number | null;
    refunded: boolean;
    reservationId: number | null;
    totalPrice: number | null;
    totalPaid: number | null;
    leftToPay: number | null;
    tipAmount: number | null;
}

const Orders: React.FC = () => {
    const [orders, setOrders] = useState<Order[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    const fetchOrders = async () => {
        try {
            const response = await axios.get(`http://localhost:5114/api/orders`, {
                params: { pageNumber: currentPage, pageSize },
                headers: { Authorization: `Bearer ${token}` },
            });
            setOrders(response.data.items);
            setTotalPages(response.data.totalPages);
            setTotalItems(response.data.totalItems);
        } catch (error) {
            console.error(ScriptResources.ErrorFetchingItems, error);
        }
    };
    
    
    useEffect(() => {
        
        fetchOrders();
        
    }, [currentPage, pageSize, token]);

    const handleIconClick = (orderId: number) => {
        navigate(`/orders/${orderId}`);
    };

    const handleDelete = async (orderId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/orders/${orderId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setOrders(orders.filter((item) => item.orderId !== orderId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
            alert(ScriptResources.ErrorDeletingItem);
        }
    };

    const handleCreateNew = async () => {
        try {
            const response = await axios.post(
                'http://localhost:5114/api/orders',
                {}, // Empty body
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            
            if (response.status === 200) {
                await fetchOrders();
            }
            
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
        }
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{verticalAlign: 'middle'}}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.OrdersList}</h2>

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>{ScriptResources.OrderId}</th>
                    <th>{ScriptResources.Status}</th>
                    <th>{ScriptResources.ReceiveTime}</th>
                    <th>{ScriptResources.CreatedBy}</th>
                    <th>{ScriptResources.Actions}</th>
                </tr>
                </thead>
                <tbody>
                {orders.map((order) => (
                    <tr key={order.orderId}
                        onDoubleClick={() => handleIconClick(order.orderId)}>
                        <td>{order.orderId}</td>
                        <td>{getOrderStatusString(order.status)}</td>
                        <td>{new Date(order.receiveTime).toLocaleString()}</td>
                        <td>{order.createdByEmployeeName}</td>
                        <td>
                                <span
                                    className="material-icons"
                                    style={{cursor: 'pointer'}}
                                    onClick={() => handleIconClick(order.orderId)}
                                >
                                    open_in_new
                                </span>
                            <span
                                className="material-icons"
                                style={{cursor: 'pointer', marginRight: '10px'}}
                                onClick={() => handleDelete(order.orderId)}
                            >
                                        delete
                                </span>
                        </td>
                    </tr>
                ))}
                </tbody>
            </Table>

            <Pagination
                currentPage={currentPage}
                totalPages={totalPages}
                totalItems={totalItems}
                pageSize={pageSize}
                onPageChange={(page) => setCurrentPage(page)}
                onPageSizeChange={(size) => setPageSize(size)}
            />
        </div>
    );
};

export default Orders;
