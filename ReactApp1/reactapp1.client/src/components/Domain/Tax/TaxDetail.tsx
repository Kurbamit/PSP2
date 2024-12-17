import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";

interface Tax {
    taxId?: number;
    percentage: number;
    description: string;
}

const TaxDetail: React.FC = () => {
    const [tax, setTax] = useState<Tax | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedTax, setEditedTax] = useState<Tax | null>(null); 

    const [error, setError] = useState<string>('');
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const token = Cookies.get('authToken');
    const isNewTax = !id;

    useEffect(() => {
        if (!isNewTax) {
            const fetchTax = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/tax/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    console.error(response.data);
                    setTax(response.data);
                    setEditedTax(response.data);

                } catch (error) {
                    console.error("Error fetching tax:", error);
                }
            };
            fetchTax();
        } else {
            const emptyTax: Tax = {
                description: '',
                percentage: 0,
            };
            setTax(emptyTax);
            setEditedTax(emptyTax);
            setIsEditing(true);
        }
    }, [id, token, isNewTax]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (editedTax) {
            const { name, value, type } = e.target;
            setEditedTax({
                ...editedTax,
                [name]: type === 'checkbox' ? e.target.checked : type === 'number' ? parseFloat(value) : value,
            });

            if (name === 'name') {
                setError('');
            }
        }
    };

    const validateForm = () => {
        if (editedTax) {
            if (editedTax.percentage === null || editedTax.percentage <= 0 || editedTax.percentage > 100) {
                setError('Percentage must be between 0 and 100.');
                return false;
            }
            if (!editedTax.description.trim()) {
                setError('Description is required.');
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
        if (validateForm() && editedTax) {
            try {
                const payload = {
                    taxId: editedTax.taxId,
                    percentage: editedTax.percentage,
                    description: editedTax.description,
                };

                if (isNewTax) {
                    await axios.post(`http://localhost:5114/api/tax`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                } else {
                    await axios.put(`http://localhost:5114/api/tax/`, payload, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
         
                navigate('/taxes');
            } catch (error) {
                console.error('Error saving tax:', error);
                setError('Failed to save tax details.');
            }
        }
    };

    const handleBackToList = () => {
        navigate('/taxes');
    };

    const handleDelete = async () => {
        try {
            if (tax?.taxId) {
                const response = await fetch(`http://localhost:5114/api/items/${tax?.taxId}`, {
                    method: 'DELETE',
                });
                if (!response.ok) {
                    throw new Error(ScriptResources.FailedToDeleteTax);
                }
                handleBackToList();
            }
        } catch (error) {
            console.error("Error deleting tax:", error);
        }
    };

    return (
        <div className="container">
            <h2 className="mb-4">{isNewTax ? ScriptResources.CreateNewTax : ScriptResources.EditTax}</h2>
            {editedTax ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{isNewTax ? ScriptResources.NewTaxInformation : ScriptResources.TaxInformation}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>{ScriptResources.ItemId}</strong> {isNewTax ? 'N/A' : tax?.taxId}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Description}</strong>{' '}
                                <input
                                    type="text"
                                    name="description"
                                    value={editedTax.description || ''}
                                    onChange={handleInputChange}
                                    className={`form-control ${error ? 'is-invalid' : ''}`}
                                    disabled={!isEditing}
                                />
                                {error && <div className="invalid-feedback">{error}</div>} {}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Percentage}</strong>{' '}
                                <input
                                    type="number"
                                    name="percentage"
                                    value={editedTax.percentage || ''}
                                    min={0}
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
                                    {!isNewTax && (
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
                                        {!isNewTax && (
                                            <button className="btn btn-danger m-1" onClick={handleDelete}>
                                                {ScriptResources.Delete}
                                            </button>
                                        )}
                                    </div>

                                </>
                            )}
                        </div>
                    </div>
                </div>
            ) : (
                <div>{ScriptResources.Loading}</div>
            )}
            {(!isEditing || isNewTax) && (
                <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                    {ScriptResources.BackToTheMainList}
                </button>
            )}

        </div>
    );
};

export default TaxDetail;
