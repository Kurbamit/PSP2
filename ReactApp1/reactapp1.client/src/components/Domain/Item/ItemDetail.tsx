import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';

interface Item {
    itemId?: number; // Make itemId optional for new items
    name: string;
    cost: number;
    tax: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: number | null;
}

const ItemDetail: React.FC = () => {
    const [item, setItem] = useState<Item | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedItem, setEditedItem] = useState<Item | null>(null); // Store edited item
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');

    const isNewItem = !id; // Check if it's a new item by absence of id

    useEffect(() => {
        if (!isNewItem) {
            const fetchItem = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/items/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    setItem(response.data);
                    setEditedItem(response.data); // Initialize edited item with fetched data
                } catch (error) {
                    console.error('Error fetching item details:', error);
                }
            };
            fetchItem();
        } else {
            const emptyItem: Item = {
                name: '',
                cost: 0,
                tax: 0,
                alcoholicBeverage: false,
                receiveTime: new Date().toISOString(),
                storage: null,
            };
            setItem(emptyItem);
            setEditedItem(emptyItem);
            setIsEditing(true); // Start in edit mode for new item
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

        if (editedItem) {
            if (!editedItem.name) {
                errorMessages.push('Name is required.');
            }
            if (editedItem.cost <= 0) {
                errorMessages.push('Cost must be greater than zero.');
            }
            if (editedItem.tax < 0) {
                errorMessages.push('Tax cannot be negative.');
            }
            if (editedItem.storage !== null && editedItem.storage < 0) {
                errorMessages.push('Storage cannot be negative.');
            }
        }

        if (errorMessages.length > 0) {
            setError(errorMessages.join(' '));
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
                    await axios.post(`http://localhost:5114/api/items`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                } else {
                    await axios.put(`http://localhost:5114/api/items/${editedItem.itemId}`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
            }
        } catch (error) {
            console.error('Error saving item details:', error);
        }
    };

    const handleBackToList = () => {
        navigate('/items');
    };

    const handleDelete = async () => {
        try {
            if (item?.itemId){
                const response = await fetch(`http://localhost:5114/api/items/${item?.itemId}`, {
                    method: 'DELETE',
                });
                if (!response.ok) {
                    throw new Error('Failed to delete the item');
                }
            }
            handleBackToList();
        } catch (error) {
            console.error('Error deleting the item:', error);
            alert("An error occurred while trying to delete the item.");
        }
    };

    return (
        <div className="container">
            <h2 className="mb-4">{isNewItem ? 'Create New Item' : 'Item Detail'}</h2>
            {editedItem ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{isNewItem ? 'New Item Information' : 'Item Information'}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>Item ID:</strong> {isNewItem ? 'N/A' : item?.itemId}
                            </li>
                            <li className="list-group-item">
                                <strong>Name:</strong>{' '}
                                <input
                                    type="text"
                                    name="name"
                                    value={editedItem.name || ''}
                                    onChange={handleInputChange}
                                    className={`form-control ${error ? 'is-invalid' : ''}`} // Add invalid class if there's an error
                                    disabled={!isEditing}
                                />
                                {error && <div className="invalid-feedback">{error}</div>} {/* Show error message */}
                            </li>
                            <li className="list-group-item">
                                <strong>Cost:</strong>{' '}
                                <input
                                    type="number"
                                    name="cost"
                                    value={editedItem.cost || 0}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>Tax:</strong>{' '}
                                <input
                                    type="number"
                                    name="tax"
                                    value={editedItem.tax || 0}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>Alcoholic Beverage:</strong>{' '}
                                <input
                                    type="checkbox"
                                    name="alcoholicBeverage"
                                    checked={editedItem.alcoholicBeverage || false}
                                    onChange={handleInputChange}
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>Receive Time:</strong> {new Date(editedItem.receiveTime).toLocaleString()}
                            </li>
                            <li className="list-group-item">
                                <strong>Storage:</strong> {editedItem.storage || 'N/A'}
                            </li>
                        </ul>
                        <div className="mt-3">
                            {isEditing ? (
                                <>
                                    <button className="btn btn-success me-2" onClick={handleFormSave}>
                                        Save
                                    </button>
                                    {!isNewItem && (
                                        <button className="btn btn-secondary" onClick={toggleEditMode}>
                                            Cancel
                                        </button>
                                    )}
                                </>
                            ) : (
                                <>
                                    <button className="btn btn-primary m-1" onClick={toggleEditMode}>
                                        Edit
                                    </button>
                                    {!isNewItem && (
                                        <button className="btn btn-danger m-1" onClick={handleDelete}>
                                            Delete
                                        </button>
                                    )}
                                </>
                            )}
                        </div>
                    </div>
                </div>
            ) : (
                <div>Loading...</div>
            )}
            {(!isEditing || isNewItem) && (
                <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                    Back to Items List
                </button>
            )}
        </div>
    );
};

export default ItemDetail;
