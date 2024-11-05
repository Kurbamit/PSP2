// src/components/Domain/Items.tsx
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Item {
    itemId: number;
    name: string;
    cost: number;
    tax: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: string | null;
    fullOrders: any[];
}

const Items: React.FC = () => {
    const [items, setItems] = useState<Item[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');

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

    return (
        <div>
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
                    </tr>
                ))}
                </tbody>
            </Table>

            {/* Use the updated Pagination component */}
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
