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
    startTime?: string;
    endTime?: string;
    createdByEmployeeId: number;
    customerPhoneNumber: string;
    establishmentId?: number;
    createdByEmployeeName?: string;
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

                const reservationsWithEmployeeNames = await Promise.all(
                    response.data.items.map(async (reservation: Reservation) => {
                        try {
                            const employeeResponse = await axios.get(
                                `http://localhost:5114/api/employees/${reservation.createdByEmployeeId}`,
                                { headers: { Authorization: `Bearer ${token}` } }
                            );
                            const employee = employeeResponse.data;
                            const employeeName = `${employee.firstName} ${employee.lastName}`;
                            return { ...reservation, createdByEmployeeName: employeeName, establishmentId: employee.establishmentId };
                        } catch (employeeError) {
                            console.error(ScriptResources.ErrorFetchingEmployeeDetailsForReservation, reservation.reservationId, employeeError);
                            return { ...reservation, createdByEmployeeName: ScriptResources.UnknownEmployee, establishmentId: null };
                        }
                    })
                );

                setReservations(reservationsWithEmployeeNames);
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
                setReservations(reservations.filter((res) => res.reservationId !== reservationId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingReservation, error);
            alert(ScriptResources.ErrorDeletingReservation);
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
                        <th>{ScriptResources.CreatedByEmployee}</th>
                        <th>{ScriptResources.CustomerPhoneNumber}</th>
                        <th>{ScriptResources.EstablishmentId}</th>
                        <th>{ScriptResources.Actions}</th>
                    </tr>
                </thead>
                <tbody>
                    {reservations.map((reservation) => (
                        <tr key={reservation.reservationId}
                            onDoubleClick={() => handleIconClick(reservation.reservationId)}>
                            <td>{reservation.receiveTime}</td>
                            <td>{reservation.startTime || ScriptResources.NotAvailable}</td>
                            <td>{reservation.endTime || ScriptResources.NotAvailable}</td>
                            <td>{reservation.createdByEmployeeName || ScriptResources.UnknownEmployee}</td>
                            <td>{reservation.customerPhoneNumber || ScriptResources.NotAvailable}</td>
                            <td>{reservation.establishmentId || ScriptResources.NotAvailable}</td>
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
