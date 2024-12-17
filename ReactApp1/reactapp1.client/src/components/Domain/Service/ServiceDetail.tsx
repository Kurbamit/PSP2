import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import { Button, Modal } from "react-bootstrap";
import SelectDropdown from "../../Base/SelectDropdown.tsx";

interface Service {
    serviceId?: number;
    name: string;
    establishmentId: number;
    serviceLength: string;
    cost?: number;
    tax?: number;
    receiveTime: string;
}
interface Tax {
    taxId: number;
    percentage: number;
    description: string;
}

const ServiceDetail: React.FC = () => {
    const [service, setService] = useState<Service | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedService, setEditedService] = useState<Service | null>(null);
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [serviceTaxes, setServiceTaxes] = useState<Tax[]>([]);
    const [showTaxModal, setShowTaxModal] = useState(false);
    const [selectedTaxId, setSelectedTaxId] = useState<number | null>(null);

    const isNewService = !id;

    useEffect(() => {
        if (!isNewService) {
            const fetchService = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/services/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    setService(response.data);
                    setEditedService(response.data);
                    getTaxes();
                } catch (error) {
                    console.error(ScriptResources.ErrorFetchingServices, error);
                }
            };
            fetchService();
        } else {
            const emptyService: Service = {
                name: '',
                establishmentId: 0,
                serviceLength: '',
                cost: undefined,
                tax: undefined,
                receiveTime: new Date().toISOString(),
            };
            setService(emptyService);
            setEditedService(emptyService);
            setIsEditing(true);
        }
    }, [id, token, isNewService]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (editedService) {
            const { name, value, type } = e.target;
            setEditedService({
                ...editedService,
                [name]: type === 'number' ? parseFloat(value) : value,
            });
            if (name === 'name') {
                setError('');
            }
        }
    };

    const toggleEditMode = () => {
        setIsEditing(!isEditing);
    };

    const validateForm = () => {
        setError('');
        const errorMessages: string[] = [];

        if (editedService) {
            if (!editedService.name) {
                errorMessages.push(ScriptResources.NameIsRequired);
            }
            if (editedService.cost && editedService.cost <= 0) {
                errorMessages.push(ScriptResources.CostMustBeGreaterThanZero);
            }
            if (editedService.tax && editedService.tax < 0) {
                errorMessages.push(ScriptResources.TaxCannotBeNegative);
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
            if (editedService) {
                if (isNewService) {
                    const response = await axios.post(`http://localhost:5114/api/services`, editedService, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const createdServiceId: number = response.data;
                    navigate(`/services/${createdServiceId}`);
                } else {
                    await axios.put(`http://localhost:5114/api/services/${editedService.serviceId}`, editedService, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
                handleBackToList();
            }
        } catch (error) {
            console.error(ScriptResources.ErrorSavingService, error);
        }
    };

    const handleBackToList = () => {
        navigate('/services');
    };

    const handleDelete = async () => {
        try {
            if (service?.serviceId) {
                const response = await axios.delete(`http://localhost:5114/api/services/${service.serviceId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (response.status !== 204) {
                    throw new Error(ScriptResources.ErrorDeletingService);
                }
            }
            handleBackToList();
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingService, error);
        }
    };

    const getTaxes = async () => {
        const response = await axios.get(`http://localhost:5114/api/tax/service/${id}/`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        setServiceTaxes(response.data);
    }

    const handleAddTax = async () => {
        try {
            await axios.post(`http://localhost:5114/api/tax/service`, {
                serviceId: service?.serviceId,
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
            await axios.delete(`http://localhost:5114/api/tax/service`, {
                headers: { Authorization: `Bearer ${token}` },
                data: {
                    serviceId: service?.serviceId,
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
            <h2 className="mb-4">{isNewService ? 'Create New Service' : 'Service Detail'}</h2>
            {editedService ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{isNewService ? ScriptResources.NewService : ScriptResources.ServiceDetail}</h5>
                        <ul className="list-group list-group-flush">
                            <li className="list-group-item">
                                <strong>{ScriptResources.ServiceId}</strong> {isNewService ? 'N/A' : service?.serviceId}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Name}</strong>{' '}
                                <input
                                    type="text"
                                    name="name"
                                    value={editedService.name || ''}
                                    onChange={handleInputChange}
                                    className={`form-control ${error ? 'is-invalid' : ''}`}
                                    disabled={!isEditing}
                                />
                                {error && <div className="invalid-feedback">{error}</div>}
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.EstablishmentId}</strong>{' '}
                                <input
                                    type="text"
                                    name="establishmentId"
                                    value={editedService.establishmentId || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={true}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ServiceLength}</strong>{' '}
                                <input
                                    type="text"
                                    name="serviceLength"
                                    value={editedService.serviceLength || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.Cost}</strong>{' '}
                                <input
                                    type="number"
                                    name="cost"
                                    value={editedService.cost || ''}
                                    onChange={handleInputChange}
                                    className="form-control"
                                    disabled={!isEditing}
                                />
                            </li>
                            <li className="list-group-item">
                                <strong>{ScriptResources.ReceiveTime}</strong> {new Date(editedService.receiveTime).toLocaleString()}
                            </li>
                        </ul>
                        <div className="mt-3">
                            {isEditing ? (
                                <>
                                    <button className="btn btn-success me-2" onClick={handleFormSave}>
                                        {ScriptResources.Save}
                                    </button>
                                    {!isNewService && (
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
                                        {!isNewService && (
                                            <button className="btn btn-danger m-1" onClick={handleDelete}>
                                                {ScriptResources.Delete}
                                            </button>
                                        )}
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                    <div className="mt-4 mb-3">
                        <h5>{ScriptResources.Taxes}</h5>
                        <ul className="list-group list-group-flush">
                            {serviceTaxes.length > 0 ? (
                                serviceTaxes.map((tax) => (
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
            <button className="btn btn-secondary mt-3" onClick={handleBackToList}>
                {ScriptResources.BackToTheMainList}
            </button>

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

export default ServiceDetail;
