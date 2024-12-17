import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

export interface Discount {
    discountId: number;
    discountName: string;
    value: number;
    validFrom: string | null;
    validTo: string | null;
}

const Discounts: React.FC = () => {
    const [discounts, setDiscounts] = useState<Discount[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    const fetchOrders = async () => {
        try {
            const response = await axios.get(`http://localhost:5114/api/discounts`, {
                params: { pageNumber: currentPage, pageSize },
                headers: { Authorization: `Bearer ${token}` },
            });
            setDiscounts(response.data.items);
            setTotalPages(response.data.totalPages);
            setTotalItems(response.data.totalItems);
        } catch (error) {
            console.error(ScriptResources.ErrorFetchingItems, error);
        }
    };


    useEffect(() => {

        fetchOrders();

    }, [currentPage, pageSize, token]);

    const handleIconClick = (discountId: number) => {
        navigate(`/discounts/${discountId}`);
    };

    const handleDelete = async (discountId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/discounts/${discountId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 200) {
                setDiscounts(discounts.filter((item) => item.discountId !== discountId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
            alert(ScriptResources.ErrorDeletingItem);
        }
    };

    const handleCreateNew = async () => {
        navigate('/discounts/new');
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
                    <th>{ScriptResources.DiscountId}</th>
                    <th>{ScriptResources.Name}</th>
                    <th>{ScriptResources.ValidFrom}</th>
                    <th>{ScriptResources.ValidTo}</th>
                    <th>{ScriptResources.Actions}</th>
                </tr>
                </thead>
                <tbody>
                {discounts.map((discount) => (
                    <tr key={discount.discountId}
                        onDoubleClick={() => handleIconClick(discount.discountId)}>
                        <td>{discount.discountId}</td>
                        <td>{discount.discountName}</td>
                        <td>{discount.validFrom === null ? '-' : new Date(discount.validFrom).toLocaleString()}</td>
                        <td>{discount.validTo === null ? '-' : new Date(discount.validTo).toLocaleString()}</td>
                        <td>
                                <span
                                    className="material-icons"
                                    style={{cursor: 'pointer'}}
                                    onClick={() => handleIconClick(discount.discountId)}
                                >
                                    open_in_new
                                </span>
                            <span
                                className="material-icons"
                                style={{cursor: 'pointer', marginRight: '10px'}}
                                onClick={() => handleDelete(discount.discountId)}
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

export default Discounts;
