import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useParams, useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import moment from 'moment';

interface GiftCard {
    giftCardId?: number;
    expirationDate: string;
    amount: number;
    code: string;
    receiveTime: string;
}

const GiftCardDetail: React.FC = () => {
    const [giftCard, setGiftCard] = useState<GiftCard | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedGiftCard, setEditedGiftCard] = useState<GiftCard | null>(null);
    const { id } = useParams<{ id: string }>();
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');

    const isNewGiftCard = !id;

    useEffect(() => {
        if (!isNewGiftCard) {
            const fetchGiftCard = async () => {
                try {
                    const response = await axios.get(`http://localhost:5114/api/giftcards/${id}`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    setGiftCard(response.data);
                    setEditedGiftCard(response.data);
                } catch (error) {
                    console.error(ScriptResources.ErrorFetchingGiftCard, error);
                }
            };
            fetchGiftCard();
        } else {
            const emptyGiftCard: GiftCard = {
                expirationDate: new Date().toISOString(),
                amount: 0,
                code: '',
                receiveTime: new Date().toISOString(),
            };
            setGiftCard(emptyGiftCard);
            setEditedGiftCard(emptyGiftCard);
            setIsEditing(true);
        }
    }, [id, token, isNewGiftCard]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (editedGiftCard) {
            const { name, value, type } = e.target;
            setEditedGiftCard({
                ...editedGiftCard,
                [name]: type === 'number' ? parseFloat(value) : value,
            });
            if (name === 'code') {
                setError('');
            }
        }
    };

    const toggleEditMode = () => {
        setIsEditing(!isEditing);
    };

    const validateForm = () => {
        const errorMessages = [];
        setError('');

        if (editedGiftCard) {
            if (!editedGiftCard.code) {
                errorMessages.push(ScriptResources.CodeIsRequired);
            }
            if (editedGiftCard.amount <= 0) {
                errorMessages.push(ScriptResources.AmountMustBeGreaterThanZero);
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
            if (editedGiftCard) {
                const formattedGiftCard = {
                    ...editedGiftCard,
                    expirationDate: moment(`${editedGiftCard.expirationDate}T00:00:00`).toISOString(),
                };
                if (isNewGiftCard) {
                    const response = await axios.post(`http://localhost:5114/api/giftcards`, formattedGiftCard, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const createdGiftCardId: number = response.data;
                    navigate(`/giftcards/${createdGiftCardId}`);
                } else {
                    await axios.put(`http://localhost:5114/api/giftcards/${editedGiftCard.giftCardId}`, formattedGiftCard, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                }
                setIsEditing(false);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorSavingGiftCard, error);
        }
    };

    const handleDelete = async () => {
        try {
            if (giftCard?.giftCardId) {
                await axios.delete(`http://localhost:5114/api/giftcards/${giftCard.giftCardId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                navigate('/giftcards');
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingGiftCard, error);
        }
    };

    const handleBackToList = () => {
        navigate('/giftcards');
    };
    
    const handleCancleEditing = () => {
        setIsEditing(false);
    }

    return (
        <div className="container">
            <h2>{isNewGiftCard ? ScriptResources.CreateNewGiftCard : ScriptResources.GiftCardDetail}</h2>
            {editedGiftCard ? (
                <div>
                    <div>
                        <label>{ScriptResources.Code}</label>
                        <input
                            type="text"
                            name="code"
                            value={editedGiftCard.code}
                            onChange={handleInputChange}
                            disabled={!isEditing}
                            className={`form-control ${error ? 'is-invalid' : ''}`}
                        />
                        {error && <div className="invalid-feedback">{error}</div>}
                    </div>
                    <div>
                        <label>{ScriptResources.Amount}</label>
                        <input
                            type="number"
                            name="amount"
                            value={editedGiftCard.amount}
                            onChange={handleInputChange}
                            disabled={!isEditing}
                            className="form-control"
                        />
                    </div>
                    <div>
                        <label>{ScriptResources.ExpirationDate}</label>
                        <input
                            type="date"
                            name="expirationDate"
                            value={editedGiftCard.expirationDate.split('T')[0]}
                            onChange={handleInputChange}
                            disabled={!isEditing}
                            className="form-control"
                        />
                    </div>
                    <div>
                        <label>{ScriptResources.ReceiveTime}</label>
                        <input
                            disabled={true}
                            className="form-control"
                        />
                    </div>

                    <div>
                        <div className="d-flex justify-content-between mb-3">
                            {isEditing ? (
                                <div>
                                    <button className="btn btn-success" onClick={handleFormSave}>
                                        {ScriptResources.Save}
                                    </button>
                                    <button className="btn btn-secondary m-1" onClick={handleCancleEditing}>
                                        {ScriptResources.Cancel}
                                    </button>
                                </div>
                            ) : (
                                <button className="btn btn-primary" onClick={toggleEditMode}>
                                    {ScriptResources.Edit}
                                </button>
                            )}
                            {!isNewGiftCard && (
                                <button className="btn btn-danger" onClick={handleDelete}>
                                    {ScriptResources.Delete}
                                </button>
                            )}
                        </div>
                        {!isEditing && (
                            <button className="btn btn-secondary" onClick={handleBackToList}>
                                {ScriptResources.BackToTheMainList}
                            </button>
                        )}
                    </div>
                </div>
            ) : (
                <div>{ScriptResources.Loading}</div>
            )}
        </div>
    );
};

export default GiftCardDetail;
