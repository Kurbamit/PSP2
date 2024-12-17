import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Discount {
    discountId?: number;
    discountName: string;
    value: number;
    validFrom: string | null;
    validTo: string | null;
}

const DiscountDetail: React.FC = () => {
    const [discount, setDiscount] = useState<Discount | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedDiscount, setEditedDiscount] = useState<Discount | null>(null);

    const [error, setError] = useState<string>('');
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const token = Cookies.get('authToken');
    const isNewDiscount = !id;

    useEffect(() => {
        if (!isNewDiscount) {
            const fetchDiscount = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/discounts/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });

                    const fetchedDiscount = response.data;

                    if (fetchedDiscount.validFrom) {
                        fetchedDiscount.validFrom = new Date(fetchedDiscount.validFrom)
                            .toISOString()
                            .split('T')[0]; // Format as YYYY-MM-DD
                    }

                    if (fetchedDiscount.validTo) {
                        fetchedDiscount.validTo = new Date(fetchedDiscount.validTo)
                            .toISOString()
                            .split('T')[0];
                    }
                    
                    setDiscount(fetchedDiscount);
                    setEditedDiscount(fetchedDiscount);
                } catch (error) {
                    console.error("Error fetching discount:", error);
                }
            };
            fetchDiscount();
        } else {
            const emptyDiscount: Discount = {
                discountName: '',
                value: 0,
                validFrom: null,
                validTo: null,
            };
            setDiscount(emptyDiscount);
            setEditedDiscount(emptyDiscount);
            setIsEditing(true);
        }
    }, [id, token, isNewDiscount]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (editedDiscount) {
            const { name, value } = e.target;
            setEditedDiscount({
                ...editedDiscount,
                [name]: name === 'value' ? parseFloat(value) : value,
            });
        }
    };

    const validateForm = () => {
        if (editedDiscount) {
            if (editedDiscount.value <= 0) {
                setError('Discount value must be greater than 0.');
                return false;
            }
            if (!editedDiscount.discountName.trim()) {
                setError('Discount name is required.');
                return false;
            }
        }
        return true;
    };

    const handleFormSave = async () => {
        if (validateForm()) {
            await handleSave();
        }
    };

    const toggleEditMode = () => {
        setIsEditing(!isEditing);
    };

    const handleSave = async () => {
        if (validateForm() && editedDiscount) {
            try {
                const payload = {
                    discountId: editedDiscount.discountId,
                    discountName: editedDiscount.discountName,
                    value: editedDiscount.value,
                    validFrom: editedDiscount.validFrom,
                    validTo: editedDiscount.validTo,
                };

                if (isNewDiscount) {
                    await axios.post(`http://localhost:5114/api/discounts`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                } else {
                    await axios.put(`http://localhost:5114/api/discounts`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
            } catch (error) {
                console.error('Error saving discount:', error);
                setError('Failed to save discount details.');
            }
        }
    };

    const handleBackToList = () => {
        navigate('/discounts');
    };

    return (
        <div className="container">
            <h2 className="mb-4">{isNewDiscount ? ScriptResources.CreateNewDiscount : ScriptResources.EditDiscount}</h2>
            {editedDiscount ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{isNewDiscount ? ScriptResources.NewDiscountInformation : ScriptResources.DiscountInformation}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>{ScriptResources.ItemId}</strong> {isNewDiscount ? 'N/A' : discount?.discountId}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.DiscountName}</strong>{' '}
                                <input
                                    type="text"
                                    name="discountName"
                                    value={editedDiscount.discountName || ''}
                                    onChange={handleInputChange}
                                    className={`form-control ${error ? 'is-invalid' : ''}`}
                                    disabled={!isEditing}
                                />
                                {error && <div className="invalid-feedback">{error}</div>}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Value}</strong>{' '}
                                <input
                                    type="number"
                                    name="value"
                                    value={editedDiscount.value || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ValidFrom}</strong>{' '}
                                <input
                                    type="date"
                                    name="validFrom"
                                    value={editedDiscount.validFrom || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ValidTo}</strong>{' '}
                                <input
                                    type="date"
                                    name="validTo"
                                    value={editedDiscount.validTo || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                        </ul>
                        <div className="mt-3">
                            {isEditing ? (
                                <>
                                    <button className="btn btn-success me-2" onClick={handleFormSave}>
                                        {ScriptResources.Save}
                                    </button>
                                    {!isNewDiscount && (
                                        <button className="btn btn-secondary" onClick={toggleEditMode}>
                                            {ScriptResources.Cancel}
                                        </button>
                                    )}
                                </>
                            ) : (
                                <>
                                    <div className="d-flex mb-2">
                                        <button className="btn btn-primary m-1" onClick={toggleEditMode}>
                                            {ScriptResources.Edit}
                                        </button>
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            ) : (
                <div>{ScriptResources.Loading}</div>
            )}
            <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                {ScriptResources.BackToTheMainList}
            </button>
        </div>
    );
};

export default DiscountDetail;
