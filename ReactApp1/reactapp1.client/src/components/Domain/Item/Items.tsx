// src/components/Domain/Item/Items.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Item {
    itemId: number;
    name: string;
    cost: number;
    tax: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: string | null;
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
                console.error('Error fetching items:', error);
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
            }
        } catch (error) {
            console.error('Error deleting the item:', error);
            alert("An error occurred while trying to delete the item.");
        }
    };

    const handleCreateNew = () => {
        navigate('/items/new');
    };
    
    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                Create New Item
            </button>
            <h2>Items List</h2>

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>Item ID</th>
                    <th>Name</th>
                    <th>Cost</th>
                    <th>Tax</th>
                    <th>Alcoholic Beverage</th>
                    <th>Receive Time</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {items.map((item) => (
                    <tr key={item.itemId}>
                        <td>{item.itemId}</td>
                        <td>{item.name}</td>
                        <td>{item.cost.toFixed(2)}</td>
                        <td>{item.tax.toFixed(2)}</td>
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
