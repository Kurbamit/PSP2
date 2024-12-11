import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Reservation {
    reservationID?: number;
    receiveTime: string;
    startTime?: string;
    endTime?: string;
    createdByEmployeeId: number;
    customerPhoneNumber: string;
    establishmentId?: number;
    createdByEmployeeName?: string;
}

const ReservationDetail: React.FC = () => {
    const [reservation, setReservation] = useState<Reservation | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedItem, setEditedItem] = useState<Reservation | null>(null);
    const [displayStartTime, setDisplayStartTime] = useState<string>('');
    const [displayEndTime, setDisplayEndTime] = useState<string>('');
    const [employeeName, setEmployeeName] = useState<string>(ScriptResources.UnknownEmployee);
    const [establishmentId, setEstablishmentId] = useState<string | number>(ScriptResources.NotAvailable);
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');

    const isNewItem = !id;

    useEffect(() => {
        if (!isNewItem) {
            const fetchItem = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/reservations/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const reservationData = response.data;
                    setReservation(reservationData);
                    setEditedItem(reservationData);
                    setDisplayStartTime(new Date(reservationData.startTime || '').toISOString().slice(0, 16));
                    setDisplayEndTime(new Date(reservationData.endTime || '').toISOString().slice(0, 16));

                    // Fetch employee details
                    try {
                        const employeeResponse = await axios.get(
                            `http://localhost:5114/api/employees/${reservationData.createdByEmployeeId}`,
                            { headers: { Authorization: `Bearer ${token}` } }
                        );
                        const employee = employeeResponse.data;
                        setEmployeeName(`${employee.firstName} ${employee.lastName}`);
                        setEstablishmentId(employee.establishmentId || ScriptResources.NotAvailable);
                    } catch (employeeError) {
                        console.error(ScriptResources.ErrorFetchingEmployeeDetails, employeeError);
                    }
                } catch (error) {
                    console.error(ScriptResources.ErrorFetchingItems, error);
                }
            };
            fetchItem();
        } else {
            const emptyReservation: Reservation = {
                receiveTime: new Date().toISOString(),
                createdByEmployeeId: 0,
                customerPhoneNumber: '',
            };
            setReservation(emptyReservation);
            setEditedItem(emptyReservation);
            setDisplayStartTime(new Date().toISOString().slice(0, 16));
            setDisplayEndTime(new Date().toISOString().slice(0, 16));
            setIsEditing(true);
        }
    }, [id, token, isNewItem]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === 'startTime') {
            setDisplayStartTime(value);
        } else if (name === 'endTime') {
            setDisplayEndTime(value);
        } else {
            setEditedItem({
                ...editedItem,
                [name]: value,
            });
        }
    };

    const toggleEditMode = () => {
        setIsEditing(!isEditing);
    };

    const validateForm = () => {
        if (!editedItem?.customerPhoneNumber) {
            setError(ScriptResources.CustomerPhoneNumberRequired);
            return false;
        }
        return true;
    };

    const handleFormSave = async () => {
        if (validateForm()) {
            await handleSave();
        }
    };

    const handleSave = async () => {
        try {
            if (editedItem) {
                const payload = {
                    ...editedItem,
                    startTime: displayStartTime + ":00.000Z",
                    endTime: displayEndTime + ":00.000Z",
                };

                if (isNewItem) {
                    const response = await axios.post(`http://localhost:5114/api/reservations`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const createdReservationId: number = response.data;
                    navigate(`/reservations/${createdReservationId}`);
                } else {
                    await axios.put(`http://localhost:5114/api/reservations/${editedItem.reservationID}`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
                setError('');
                handleBackToList();
            }
        } catch (error) {
            console.error(ScriptResources.ErrorSavingItem, error);
        }
    };

    const handleBackToList = () => {
        navigate('/reservations');
    };

    const handleDelete = async () => {
        try {
            if (reservation?.reservationID != null) {
                const response = await fetch(`http://localhost:5114/api/reservations/${reservation?.reservationID}`, {
                    method: 'DELETE',
                });
                if (!response.ok) {
                    throw new Error(ScriptResources.FailedToDeleteItem);
                }
            }
            handleBackToList();
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
            alert(ScriptResources.ErrorDeletingItem);
        }
    };

    return (
        <div className="container-fluid">
            <h2 className="mb-4">{isNewItem ? 'Create New Reservation' : 'Reservation Detail'}</h2>
            {editedItem ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">
                            {isNewItem ? ScriptResources.NewReservationInformation : ScriptResources.ReservationInformation}
                        </h5>
                        <div className="row">
                            <div className="col-12">
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.ReceiveTime}</strong>{' '}
                                        <input
                                            type="text"
                                            name="receiveTime"
                                            value={new Date(editedItem.receiveTime).toLocaleString()}
                                            disabled={true}
                                            className="form-control"
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.StartTime}</strong>{' '}
                                        <input
                                            type="datetime-local"
                                            name="startTime"
                                            value={displayStartTime}
                                            onChange={handleInputChange}
                                            className="form-control"
                                            disabled={!isEditing}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.EndTime}</strong>{' '}
                                        <input
                                            type="datetime-local"
                                            name="endTime"
                                            value={displayEndTime}
                                            onChange={handleInputChange}
                                            className="form-control"
                                            disabled={!isEditing}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.CustomerPhoneNumber}</strong>{' '}
                                        <input
                                            type="text"
                                            name="customerPhoneNumber"
                                            value={editedItem.customerPhoneNumber || ''}
                                            onChange={handleInputChange}
                                            className="form-control"
                                            disabled={!isEditing}
                                        />
                                    </li>
                                    {!isNewItem && (
                                        <>
                                            <li className="list-group-item">
                                                <strong>{ScriptResources.CreatedByEmployee}</strong>{' '}
                                                <input
                                                    type="text"
                                                    value={employeeName}
                                                    disabled={true}
                                                    className="form-control"
                                                />
                                            </li>
                                            <li className="list-group-item">
                                                <strong>{ScriptResources.EstablishmentId}</strong>{' '}
                                                <input
                                                    type="text"
                                                    value={establishmentId}
                                                    disabled={true}
                                                    className="form-control"
                                                />
                                            </li>
                                        </>
                                    )}
                                </ul>
                            </div>
                        </div>
                        <div className="mt-3">
                            {isEditing ? (
                                <>
                                    <button className="btn btn-success me-2" onClick={handleFormSave}>
                                        {ScriptResources.Save}
                                    </button>
                                    {!isNewItem && (
                                        <button className="btn btn-secondary" onClick={toggleEditMode}>
                                            {ScriptResources.Cancel}
                                        </button>
                                    )}
                                </>
                            ) : (
                                <div className="d-flex mb-2">
                                    <button className="btn btn-primary m-1" onClick={toggleEditMode}>
                                        {ScriptResources.Edit}
                                    </button>
                                    {!isNewItem && (
                                        <button className="btn btn-danger m-1" onClick={handleDelete}>
                                            {ScriptResources.Delete}
                                        </button>
                                    )}
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            ) : (
                <p>{ScriptResources.Loading}</p>
            )}
            {(!isEditing || isNewItem) && (
                <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
            )}
        </div>
    );
};

export default ReservationDetail;
