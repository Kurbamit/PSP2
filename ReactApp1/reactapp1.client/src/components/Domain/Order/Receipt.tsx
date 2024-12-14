import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from "js-cookie";
import {useNavigate, useParams} from "react-router-dom";
import ScriptResources from "../../../assets/resources/strings.ts";
import { Order } from "./Orders.tsx";
import {getOrderStatusString} from "../../../assets/Utils/utils.ts";


interface Item {
    itemId: number;
    name: string;
    cost?: number | null;
    tax?: number | null;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage?: number | null;
    count?: number | null;
}

interface Payment {
    // Define payment fields here if needed
}

interface OrderItemsPayments {
    order: Order;
    items: Item[];
    payments: Payment[];
}

const Receipt: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [data, setData] = useState<OrderItemsPayments | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const token = Cookies.get('authToken');

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const response = await axios.get(`http://localhost:5114/api/orders/${id}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                setData(response.data);
            } catch (err: any) {
                setError(err.message || 'An error occurred');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [id]);

    const handleBackToList = () => {
        navigate(`/orders/${id}`);
    };
    
    const handleDownload = async () => {
        try {
            const response = await axios.get(`http://localhost:5114/api/orders/${id}/download`, {
                headers: { Authorization: `Bearer ${token}` },
                responseType: 'blob', // Ensures the response is treated as a binary file
            });

            const blob = new Blob([response.data], { type: 'text/plain' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Receipt_Order_${id}.txt`;
            link.click();
            window.URL.revokeObjectURL(url); // Clean up
        }catch (err) {
            console.error('Error downloading receipt:', err);
        }
    }

    if (loading) return <div>{ScriptResources.Loading}</div>;
    if (error) return <div>{ScriptResources.Error} + {error}</div>;

    if (!data) return <div>{ScriptResources.NoDataFound}</div>;

    const { order, items } = data;

    return (
        <div className="container mt-5">
            <h1 className="mb-4">{ScriptResources.Receipt}</h1>
            <div className="card mb-4">
                <div className="card-body">
                    <h5 className="card-title">{ScriptResources.OrderDetails}</h5>
                    <p><strong>{ScriptResources.OrderId}</strong> {order.orderId}</p>
                    <p><strong>{ScriptResources.Status}</strong> {getOrderStatusString(order.status)}</p>
                    <p><strong>{ScriptResources.Employee}</strong> {order.createdByEmployeeName}</p>
                    <p><strong>{ScriptResources.ReceiveTime}</strong> {new Date(order.receiveTime).toLocaleString()}</p>
                    <p><strong>{ScriptResources.TotalPrice}</strong> {order.totalPrice?.toFixed(2) ?? 'N/A'} {ScriptResources.Euro}</p>
                    <p><strong>{ScriptResources.TotalPaid}</strong> {order.totalPaid?.toFixed(2) ?? 'N/A'} {ScriptResources.Euro}</p>
                </div>
            </div>

            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{ScriptResources.Items}</h5>
                    <table className="table table-striped">
                        <thead>
                        <tr>
                            <th>{ScriptResources.Name}</th>
                            <th>{ScriptResources.Cost}</th>
                            <th>{ScriptResources.Tax}</th>
                            <th>{ScriptResources.AlcoholicBeverage}</th>
                            <th>{ScriptResources.Count}</th>
                        </tr>
                        </thead>
                        <tbody>
                        {items.map(item => (
                            <tr key={item.itemId}>
                                <td>{item.name}</td>
                                <td>{item.cost?.toFixed(2) ?? 'N/A'} {ScriptResources.Euro}</td>
                                <td>{item.tax?.toFixed(2) ?? 'N/A'} {ScriptResources.Euro}</td>
                                <td>{item.alcoholicBeverage ? ScriptResources.Yes : ScriptResources.No}</td>
                                <td>{item.count ?? 'N/A'}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            </div>
            <div className="d-flex justify-content-between mt-3">
                <button className="btn btn-secondary" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
                <button className="btn btn-primary" onClick={handleDownload}>
                    {ScriptResources.Download}
                </button>
            </div>
        </div>
    );
};

export default Receipt;