import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Service {
    serviceId: number;
    establishmentId: number;
    serviceLength: string; // TimeSpan formatted string
    cost?: number;
    tax?: number;
    receiveTime: string; // DateTime formatted string
}

const Services: React.FC = () => {
    const [services, setServices] = useState<Service[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchServices = async () => {
            try {
                const response = await axios.get(`http://localhost:5114/api/services`, {
                    params: { pageNumber: currentPage, pageSize },
                    headers: { Authorization: `Bearer ${token}` },
                });

                setServices(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalItems(response.data.totalItems);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingServices, error);
            }
        };

        fetchServices();
    }, [currentPage, pageSize, token]);

    const handleIconClick = (serviceId: number) => {
        navigate(`/services/${serviceId}`);
    };

    const handleDelete = async (serviceId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/services/${serviceId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setServices(services.filter((service) => service.serviceId !== serviceId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingService, error);
            alert(ScriptResources.ErrorDeletingService);
        }
    };

    const handleCreateNew = () => {
        navigate('/services/new');
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.ServicesList}</h2>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>{ScriptResources.EstablishmentId}</th>
                        <th>{ScriptResources.ServiceLength}</th>
                        <th>{ScriptResources.Cost}</th>
                        <th>{ScriptResources.Tax}</th>
                        <th>{ScriptResources.ReceiveTime}</th>
                        <th>{ScriptResources.Actions}</th>
                    </tr>
                </thead>
                <tbody>
                    {services.map((service) => (
                        <tr key={service.serviceId}
                            onDoubleClick={() => handleIconClick(service.serviceId)}>
                            <td>{service.establishmentId}</td>
                            <td>{service.serviceLength}</td>
                            <td>{service.cost ? `$${service.cost.toFixed(2)}` : ScriptResources.NotAvailable}</td>
                            <td>{service.tax ? `$${service.tax.toFixed(2)}` : ScriptResources.NotAvailable}</td>
                            <td>{service.receiveTime}</td>
                            <td style={{ display: 'flex', justifyContent: 'space-around' }}>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginRight: '5px' }}
                                    onClick={() => handleIconClick(service.serviceId)}
                                >
                                    open_in_new
                                </span>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginLeft: '5px' }}
                                    onClick={() => handleDelete(service.serviceId)}
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

export default Services;
