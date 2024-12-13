import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Reservation {
    reservationId: number;
    receiveTime: string;
    startTime: string;
    endTime: string;
    createdByEmployeeId: number;
    establishmentId: number;
    establishmentAddressId: number;
    serviceId: number;
    customerPhoneNumber: string;
}

const Reservations: React.FC = () => {
    const [reservations, setReservations] = useState<Reservation[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const response = await axios.get(`http://localhost:5114/api/reservations`, {
                    params: { pageNumber: currentPage, pageSize },
                    headers: { Authorization: `Bearer ${token}` },
                });

                setReservations(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalItems(response.data.totalItems);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingReservations, error);
            }
        };

        fetchReservations();
    }, [currentPage, pageSize, token]);

    const handleIconClick = (reservationId: number) => {
        navigate(`/reservations/${reservationId}`);
    };

    const handleDelete = async (reservationId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/reservations/${reservationId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setReservations(reservations.filter((reservation) => reservation.reservationId !== reservationId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingReservation, error);
        }
    };

    const handleCreateNew = () => {
        navigate('/reservations/new');
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.ReservationsList}</h2>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>{ScriptResources.ReceiveTime}</th>
                        <th>{ScriptResources.StartTime}</th>
                        <th>{ScriptResources.EndTime}</th>
                        <th>{ScriptResources.CreatedByEmployeeId}</th>
                        <th>{ScriptResources.EstablishmentId}</th>
                        <th>{ScriptResources.EstablishmentAddressId}</th>
                        <th>{ScriptResources.ServiceId}</th>
                        <th>{ScriptResources.CustomerPhoneNumber}</th>
                        <th>{ScriptResources.Actions}</th>
                    </tr>
                </thead>
                <tbody>
                    {reservations.map((reservation) => (
                        <tr key={reservation.reservationId}
                            onDoubleClick={() => handleIconClick(reservation.reservationId)}>
                            <td>{new Date(reservation.receiveTime).toLocaleString()}</td>
                            <td>{new Date(reservation.startTime).toLocaleString()}</td>
                            <td>{new Date(reservation.endTime).toLocaleString()}</td>
                            <td>{reservation.createdByEmployeeId}</td>
                            <td>{reservation.establishmentId}</td>
                            <td>{reservation.establishmentAddressId}</td>
                            <td>{reservation.serviceId}</td>
                            <td>{reservation.customerPhoneNumber}</td>
                            <td style={{ display: 'flex', justifyContent: 'space-around' }}>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginRight: '5px' }}
                                    onClick={() => handleIconClick(reservation.reservationId)}
                                >
                                    open_in_new
                                </span>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginLeft: '5px' }}
                                    onClick={() => handleDelete(reservation.reservationId)}
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

export default Reservations;
