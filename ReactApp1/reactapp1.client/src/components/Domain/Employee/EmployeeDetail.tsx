import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Employee {
    employeeId?: number; // Make employeeId optional for new items
    establishmentId: number;
    title: number;
    personalCode: string;
    firstName: string;
    lastName: string;
    birthDate: Date;
    phone: string;
    email: string;
    country: string;
    city: string;
    street: string;
    streetNumber: string;
    houseNumber: string;
    receiveTime: string;
    passwordHash: string;
}

const EmployeeDetail: React.FC = () => {
    const [employee, setEmployee] = useState<Employee | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedItem, setEditedItem] = useState<Employee | null>(null); // Store edited employee
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [emailError, setEmailError] = useState('');

    const isNewItem = !id; // Check if it's a new employee by absence of id

    useEffect(() => {
        if (!isNewItem) {
            const fetchItem = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/employees/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    setEmployee(response.data);
                    setEditedItem(response.data); // Initialize edited employee with fetched data
                } catch (error) {
                    console.error(ScriptResources.ErrorFetchingItems, error);
                }
            };
            fetchItem();
        } else {
            const emptyEmployee: Employee = {
                establishmentId: 0,
                title: 0,
                personalCode: '',
                firstName: '',
                lastName: '',
                birthDate: new Date(),
                phone: '',
                email: '',
                country: '',
                city: '',
                street: '',
                streetNumber: '',
                houseNumber: '',
                receiveTime: new Date().toISOString(),
                passwordHash: '',
            };
            setEmployee(emptyEmployee);
            setEditedItem(emptyEmployee);
            setIsEditing(true); // Start in edit mode for new employee
        }
    }, [id, token, isNewItem]);

    // Handle field changes in edit mode
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (editedItem) {
            const { name, value, type } = e.target;
            setEditedItem({
                ...editedItem,
                [name]: type === 'checkbox' ? e.target.checked : type === 'number' ? parseFloat(value) : value,
            });
            // Clear error when user starts typing
            if (name === 'name') {
                setError('');
            }
        }
    };

    // Toggle edit mode
    const toggleEditMode = () => {
        setIsEditing(!isEditing);
    };

    const validateForm = () => {
        setError('');
        const errorMessages = [];
        const emailError = [];

        if (editedItem) {
            if (!editedItem.firstName) {
                errorMessages.push(ScriptResources.NameIsRequired);
            }
        }

        if (!editedItem?.email || editedItem.email.length === 0) {
            emailError.push(ScriptResources.EmailIsRequired) // If email is empty, show this error
        } else {
            // Email format validation using a regular expression
            const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            if (!emailPattern.test(editedItem.email)) {
                emailError.push(ScriptResources.InvalidEmail)  // If email is invalid, show this error
            }
        }

        if (errorMessages.length > 0 || emailError.length > 0) {
            setError(errorMessages.join(' '));
            setEmailError(emailError.join(' '));
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
                if (isNewItem) {
                    const response = await axios.post(`http://localhost:5114/api/employees`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const createdEmployeeId: number = response.data;
                    navigate(`/employees/${createdEmployeeId}`);
                } else {
                    await axios.put(`http://localhost:5114/api/employees/${editedItem.employeeId}`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
                setEmailError('');
                setError('');
            }
        } catch (error) {
            console.error(ScriptResources.ErrorSavingItem, error);
        }
    };

    const handleBackToList = () => {
        navigate('/employees');
    };
    
    const handleDelete = async () => {
        try {
            if (employee?.employeeId){
                const response = await fetch(`http://localhost:5114/api/employees/${employee?.employeeId}`, {
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
        <div className="container-fluid"> {/* Use container-fluid for a wider layout */}
            <h2 className="mb-4">{isNewItem ? 'Create New Employee' : 'Employee Detail'}</h2>
            {editedItem ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">
                            {isNewItem ? ScriptResources.NewEmployeeInformation : ScriptResources.EmployeesInformation}
                        </h5>
                        <div className="row"> {/* Start a new row */}
                            <div className="col-md-6"> {/* First column */}
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.PersonalCode}</strong>{' '}
                                        <input
                                            type="text"
                                            name="personalCode"
                                            value={editedItem.personalCode || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Name}</strong>{' '}
                                        <input
                                            type="text"
                                            name="firstName"
                                            value={editedItem.firstName || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.LastName}</strong>{' '}
                                        <input
                                            type="text"
                                            name="lastName"
                                            value={editedItem.lastName || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.BirthDate}</strong>{' '}
                                        <input
                                            type="date"
                                            name="birthDate"
                                            value={
                                                editedItem.birthDate
                                                    ? new Date(editedItem.birthDate).toISOString().split('T')[0]
                                                    : ''
                                            }
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.PhoneNumber}</strong>{' '}
                                        <input
                                            type="text"
                                            name="phone"
                                            value={editedItem.phone || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.EmployeeEmail}</strong>{' '}
                                        <input
                                            type="email"
                                            name="email"
                                            value={editedItem.email || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${emailError ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                            required={true}
                                        />
                                        {emailError && <div className="invalid-feedback">{emailError}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Password}</strong>{' '}
                                        <input
                                            type="password"
                                            name="passwordHash"
                                            value={editedItem.passwordHash || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                </ul>
                            </div>
                            <div className="col-md-6"> {/* Second column */}
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Country}</strong>{' '}
                                        <input
                                            type="text"
                                            name="country"
                                            value={editedItem.country || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.City}</strong>{' '}
                                        <input
                                            type="text"
                                            name="city"
                                            value={editedItem.city || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Street}</strong>{' '}
                                        <input
                                            type="text"
                                            name="street"
                                            value={editedItem.street || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.StreetNumber}</strong>{' '}
                                        <input
                                            type="text"
                                            name="streetNumber"
                                            value={editedItem.streetNumber || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.HouseNumber}</strong>{' '}
                                        <input
                                            type="text"
                                            name="houseNumber"
                                            value={editedItem.houseNumber || ''}
                                            onChange={handleInputChange}
                                            className={`form-control ${error ? 'is-invalid' : ''}`}
                                            disabled={!isEditing}
                                        />
                                        {error && <div className="invalid-feedback">{error}</div>}
                                    </li>
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
                <div>{ScriptResources.Loading}</div>
            )}
            {(!isEditing || isNewItem) && (
                <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
            )}
        </div>
    );

};

export default EmployeeDetail;
