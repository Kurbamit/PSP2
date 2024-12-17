import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import { Button, Modal } from "react-bootstrap";
import SelectDropdown from "../../Base/SelectDropdown.tsx";

interface Item {
    itemId?: number; // Make itemId optional for new items
    name: string;
    cost: number;
    alcoholicBeverage: boolean;
    receiveTime: string;
    storage: number | null;
    baseItemId: number;
}
interface Tax {
    taxId: number;
    percentage: number;
    description: string;
}

const ItemDetail: React.FC = () => {
    const [item, setItem] = useState<Item | null>(null);
    const [baseItem, setBaseItem] = useState<Item | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedItem, setEditedItem] = useState<Item | null>(null); // Store edited item
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [modalError, setModalError] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [storageValue, setStorageValue] = useState<number | ''>('');
    const [modalMode, setModalMode] = useState<'add' | 'deduct'>('add');
    const [itemTaxes, setItemTaxes] = useState<Tax[]>([]);
    const [showTaxModal, setShowTaxModal] = useState(false);
    const [selectedTaxId, setSelectedTaxId] = useState<number | null>(null);
    const [baseItemId, setBaseItemId] = useState<number>(0);

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
                    getTaxes();

                    const baseItemResponse = await axios.get(`http://localhost:5114/api/items/${response.data.baseItemId}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    setBaseItem(baseItemResponse.data);

                } catch (error) {
                    console.error(ScriptResources.ErrorFetchingItems, error);
                }
            };
            fetchItem();
        } else {
            const emptyItem: Item = {
                name: '',
                cost: 0,
                alcoholicBeverage: false,
                receiveTime: new Date().toISOString(),
                storage: null,
                baseItemId: 0,
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
                errorMessages.push(ScriptResources.NameIsRequired);
            }
            if (editedItem.cost <= 0) {
                errorMessages.push(ScriptResources.CostMustBeGreaterThanZero);
            }
            if (editedItem.storage !== null && editedItem.storage < 0) {
                errorMessages.push(ScriptResources.StorageCannotBeEmpty);
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
                    const response = await axios.post(`http://localhost:5114/api/items`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const createdItemId: number = response.data;
                    navigate(`/items/${createdItemId}`);
                } else {
                    await axios.put(`http://localhost:5114/api/items/${editedItem.itemId}`, editedItem, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }

                const baseItemResponse = await axios.get(`http://localhost:5114/api/items/${editedItem.baseItemId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                setBaseItem(baseItemResponse.data);

                setIsEditing(false);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorSavingItem, error);
            setBaseItemId(item?.baseItemId ? item.baseItemId : 0)
        }
    };

    const handleBackToList = () => {
        navigate('/items');
    };

    const handleSaveStorage = async () => {
        if (typeof storageValue === 'number' && storageValue < 0) {
            setModalError('Storage value cannot be negative');
            return;
        }
        if (editedItem && storageValue !== '') {
            try {
                const endpoint =
                    modalMode === 'add'
                        ? `http://localhost:5114/api/items/${editedItem.itemId}/add-storage?amount=${storageValue}`
                        : `http://localhost:5114/api/items/${editedItem.itemId}/deduct-storage?amount=${storageValue}`;

                await axios.put(endpoint, {}, { headers: { Authorization: `Bearer ${token}` } });

                setEditedItem({
                    ...editedItem,
                    storage:
                        modalMode === 'add'
                            ? (editedItem.storage || 0) + (typeof storageValue === 'number' ? storageValue : 0)
                            : (editedItem.storage || 0) - (typeof storageValue === 'number' ? storageValue : 0),
                });

                setShowModal(false);
                setStorageValue('');
            } catch (error) {
                console.error(ScriptResources.Error, error);
            }
        }
    };

    const openAddStorageModal = () => {
        setModalMode('add');
        setShowModal(true);
    };

    const openDeductStorageModal = () => {
        setModalMode('deduct');
        setShowModal(true);
    };

    const handleDelete = async () => {
        try {
            if (item?.itemId){
                const response = await fetch(`http://localhost:5114/api/items/${item?.itemId}`, {
                    method: 'DELETE',
                });
                if (!response.ok) {
                    throw new Error(ScriptResources.FailedToDeleteItem);
                }
            }
            handleBackToList();
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingItem, error);
        }
    };

    const getTaxes = async () => {
        const response = await axios.get(`http://localhost:5114/api/tax/item/${id}/`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        setItemTaxes(response.data);
    }

    const handleAddTax = async () => {
        try {
            await axios.post(`http://localhost:5114/api/tax/item`, {
                itemId: item?.itemId,
                taxId: selectedTaxId,
            }, {
                headers: { Authorization: `Bearer ${token}` },
            });
            
            getTaxes()
            setShowTaxModal(false);
        } catch (error) {
            console.error("Error adding tax:", error);
        }
    };

    const handleDeleteTax = async (taxId: number) => {
        try {
            await axios.delete(`http://localhost:5114/api/tax/item`, {
                headers: { Authorization: `Bearer ${token}` },
                data: {
                    itemId: item?.itemId,
                    taxId: taxId
                },
            });
            getTaxes();
        } catch (error) {
            console.error("Error deleting tax:", error);
        }
    };
    return (
        <div className="container">
            <h2 className="mb-4">{isNewItem ? 'Create New Item' : 'Item Detail'}</h2>
            {editedItem ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{isNewItem ? ScriptResources.NewItemInformation : ScriptResources.ItemInformation}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>{ScriptResources.ItemId}</strong> {isNewItem ? 'N/A' : item?.itemId}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Name}</strong>{' '}
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
                                <strong>{ScriptResources.Cost}</strong>{' '}
                                <input
                                    type="number"
                                    name="cost"
                                    value={editedItem.cost || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.BaseItem}</strong>
                                <SelectDropdown
                                    endpoint="/AllBaseItems" 
                                    onSelect={(item) => {
                                        if (item) {
                                            setEditedItem((prev) => (prev ? { ...prev, baseItemId: item.id } : null));
                                        }
                                    }}
                                    disabled={!isEditing} 
                                />
                                <div className="mt-2">
                                    {baseItem?.name
                                        ? `${ScriptResources.SelectedBaseItem}: ${baseItem.name}`
                                        : ScriptResources.ThisIsABaseItem}
                                </div>
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.AlcoholicBeverage}</strong>{' '}
                                <input
                                    type="checkbox"
                                    name="alcoholicBeverage"
                                    checked={editedItem.alcoholicBeverage || false}
                                    onChange={handleInputChange}
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ReceiveTime}</strong> {new Date(editedItem.receiveTime).toLocaleString()}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Storage}</strong> {editedItem.storage || 'N/A'}
                            </li>
                        </ul>
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
                                <>
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

                                    <div className="d-flex mb-2">
                                        <button className="btn btn-info m-1" onClick={openAddStorageModal}>
                                            <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>add</span>
                                            {ScriptResources.AddStorage}
                                        </button>
                                        <button className="btn btn-warning m-1" onClick={openDeductStorageModal}>
                                            <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>remove</span>
                                            {ScriptResources.DeductStorage}
                                        </button>
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                    <div className="mt-4 mb-3">
                        <h5>{ScriptResources.Taxes}</h5>
                        <ul className="list-group list-group-flush">
                            {itemTaxes.length > 0 ? (
                                itemTaxes.map((tax) => (
                                    <li key={tax.taxId} className="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>{tax.description}</strong>: {tax.percentage}%
                                        </div>
                                        <button
                                            className="btn btn-danger btn-sm"
                                            onClick={() => handleDeleteTax(tax.taxId)}
                                        >
                                            {ScriptResources.Delete}
                                        </button>
                                    </li>
                                ))
                            ) : (
                                <li className="list-group-item">{ScriptResources.NoTaxes}</li>
                            )}
                        </ul>
                        <button
                            className="btn btn-primary mt-3"
                            onClick={() => setShowTaxModal(true)}
                        >
                            {ScriptResources.AddNewTax}
                        </button>
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

            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>{ScriptResources.AddStorageNumber}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <input
                        type="number"
                        value={storageValue}
                        min={1}
                        onChange={(e) => setStorageValue(e.target.value === '' ? '' : parseInt(e.target.value))}
                        className="form-control"
                        placeholder={ScriptResources.StorageAdded}
                    />
                    {modalError && <div style={{ color: 'red', marginTop: '10px' }}>{modalError}</div>}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        {ScriptResources.Cancel}
                    </Button>
                    <Button variant="primary" onClick={handleSaveStorage}>
                        {ScriptResources.Save}
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showTaxModal} onHide={() => setShowTaxModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>{ScriptResources.AddNewTax}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <SelectDropdown
                        endpoint="/AllTaxes"
                        onSelect={(tax) => {
                            if (tax) {
                                setSelectedTaxId(tax.id);
                            }
                        }}
                        disabled={false}
                    />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowTaxModal(false)}>
                        {ScriptResources.Cancel}
                    </Button>
                    <Button variant="primary" onClick={handleAddTax}>
                        {ScriptResources.AddTax}
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default ItemDetail;
