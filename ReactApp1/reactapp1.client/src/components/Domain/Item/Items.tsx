// src/components/Domain/Item/Items.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Item {
    itemId: number;
    name: string;
    cost: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: string | null;
    baseItemId: number;
}

const Items: React.FC = () => {
    const [items, setItems] = useState<Item[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    useEffect(() => {
        const fetchItems = async () => {
            try {
                const response = await axios.get(`http://localhost:5114/api/items`, {
                    params: { pageNumber: currentPage, pageSize },
                    headers: { Authorization: `Bearer ${token}` },
                });
                setItems(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalItems(response.data.totalItems);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingItems, error);
            }
        };

        fetchItems();
    }, [currentPage, pageSize, token]);

    const handleIconClick = (itemId: number) => {
        navigate(`/items/${itemId}`);
    };

    const handleDelete = async (itemId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/items/${itemId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            
            if (response.status === 204) {
                setItems(items.filter((item) => item.itemId !== itemId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
        }
    };

    const handleCreateNew = () => {
        navigate('/items/new');
    };
    
    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{verticalAlign: 'middle'}}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.ItemsList}</h2>

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>{ScriptResources.ItemId}</th>
                    <th>{ScriptResources.BaseItemId}</th>
                    <th>{ScriptResources.Name}</th>
                    <th>{ScriptResources.Cost}</th>
                    <th>{ScriptResources.AlcoholicBeverage}</th>
                    <th>{ScriptResources.ReceiveTime}</th>
                    <th>{ScriptResources.Actions}</th>
                </tr>
                </thead>
                <tbody>
                {items.map((item) => (
                    <tr key={item.itemId}
                        onDoubleClick={() => handleIconClick(item.itemId)}>
                        <td>{item.itemId}</td>
                        <td>{item.baseItemId === 0 ? '-' : item.baseItemId}</td>
                        <td>{item.name}</td>
                        <td>{item.cost.toFixed(2)}</td>
                        <td>{item.alcoholicBeverage ? 'Yes' : 'No'}</td>
                        <td>{new Date(item.receiveTime).toLocaleString()}</td>
                        <td>
                                <span
                                    className="material-icons"
                                    style={{cursor: 'pointer'}}
                                    onClick={() => handleIconClick(item.itemId)}
                                >
                                    open_in_new
                                </span>
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

export default Items;
