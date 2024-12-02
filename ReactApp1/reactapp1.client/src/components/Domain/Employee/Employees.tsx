// src/components/Domain/Item/Items.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Employee {
    employeeId: number;
    title: number;
    establishmentId: number;
    personalCode: string;
    firstName: string;
    lastName: string;
    phone: string;
    email: string;
}

const Items: React.FC = () => {
    const [items, setItems] = useState<Employee[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    useEffect(() => {
        const fetchItems = async () => {
            try {
                const response = await axios.get(`http://localhost:5114/api/employees`, {
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
        navigate(`/employees/${itemId}`);
    };

    const handleDelete = async (employeeId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/employees/${employeeId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setItems(items.filter((item) => item.employeeId !== employeeId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
            alert(ScriptResources.ErrorDeletingItem);
        }
    };

    const handleCreateNew = () => {
        navigate('/employees/new');
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{verticalAlign: 'middle'}}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.EmployeesList}</h2>

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>{ScriptResources.Name}</th>
                    <th>{ScriptResources.LastName}</th>
                    <th>{ScriptResources.Email}</th>
                    <th>{ScriptResources.PhoneNumber}</th>
                    <th>{ScriptResources.Actions}</th>
                </tr>
                </thead>
                <tbody>
                {items.map((item) => (
                    <tr key={item.employeeId}>
                        <td>{item.firstName}</td>
                        <td>{item.lastName}</td>
                        <td>{item.email}</td>
                        <td>{item.phone}</td>
                        <td>
                                <span
                                    className="material-icons"
                                    style={{cursor: 'pointer'}}
                                    onClick={() => handleIconClick(item.employeeId)}
                                >
                                    open_in_new
                                </span>
                            <span
                                className="material-icons"
                                style={{cursor: 'pointer', marginRight: '10px'}}
                                onClick={() => handleDelete(item.employeeId)}
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
