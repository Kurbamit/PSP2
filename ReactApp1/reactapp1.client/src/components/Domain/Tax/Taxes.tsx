import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Tax {
    taxId: number;
    percentage: number; // Tax percentage, e.g., 10 for 10%
    description: string; // Description of the tax
}

const Taxes: React.FC = () => {
    const [taxes, setTaxes] = useState<Tax[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchTaxes = async () => {
            try {
                const response = await axios.get('http://localhost:5114/api/tax', {
                    params: { pageNumber: currentPage, pageSize },
                    headers: { Authorization: `Bearer ${token}` },
                });

                setTaxes(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalItems(response.data.totalItems);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingTaxes, error);
            }
        };

        fetchTaxes();
    }, [currentPage, pageSize, token]);

    const handleEditClick = (taxId: number) => {
        navigate(`/taxes/${taxId}`);
    };

    const handleDelete = async (taxId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/tax/${taxId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setTaxes(taxes.filter((tax) => tax.taxId !== taxId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error("Error deleting tax:", error);
        }
    };

    const handleCreateNew = () => {
        navigate('/taxes/new');
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.TaxesList}</h2>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>{ScriptResources.TaxId}</th>
                        <th>{ScriptResources.Percentage}</th>
                        <th>{ScriptResources.Description}</th>
                        <th>{ScriptResources.Actions}</th>
                    </tr>
                </thead>
                <tbody>
                    {taxes.map((tax) => (
                        <tr key={tax.taxId}
                            onDoubleClick={() => handleEditClick(tax.taxId)}>
                            <td>{tax.taxId}</td>
                            <td>{`${tax.percentage}%`}</td>
                            <td>{tax.description}</td>
                            <td style={{ display: 'flex', justifyContent: 'space-around' }}>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginRight: '5px' }}
                                    onClick={() => handleEditClick(tax.taxId)}
                                >
                                    edit
                                </span>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginLeft: '5px' }}
                                    onClick={() => handleDelete(tax.taxId)}
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

export default Taxes;
