import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Table } from 'react-bootstrap';
import Pagination from '../../Base/Pagination';
import 'bootstrap/dist/css/bootstrap.min.css';
import ScriptResources from "../../../assets/resources/strings.ts";

interface GiftCard {
    giftCardId?: number;
    expirationDate: string;
    amount: number;
    code: string;
    receiveTime: string;
}

const GiftCards: React.FC = () => {
    const [giftCards, setGiftCards] = useState<GiftCard[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalItems, setTotalItems] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const token = Cookies.get('authToken');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchGiftCards = async () => {
            try {
                const response = await axios.get(`http://localhost:5114/api/giftcards`, {
                    params: { pageNumber: currentPage, pageSize },
                    headers: { Authorization: `Bearer ${token}` },
                });
                setGiftCards(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalItems(response.data.totalItems);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingGiftCards, error);
            }
        };

        fetchGiftCards();
    }, [currentPage, pageSize, token]);

    const handleIconClick = (giftCardId?: number) => {
        navigate(`/giftcards/${giftCardId}`);
    };

    const handleDelete = async (giftCardId?: number) => {
        try {
            const response = await axios.delete(`http://localhost:5114/api/giftcards/${giftCardId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            if (response.status === 204) {
                setGiftCards(giftCards.filter((giftCard) => giftCard.giftCardId !== giftCardId));
                setTotalItems(totalItems - 1);
            }
        } catch (error) {
            console.error(ScriptResources.ErrorDeletingGiftCard, error);
        }
    };

    const handleCreateNew = () => {
        navigate('/giftcards/new');
    };

    return (
        <div>
            <button className="btn btn-primary mb-3" onClick={handleCreateNew}>
                <span className="material-icons me-2" style={{ verticalAlign: 'middle' }}>add</span>
                {ScriptResources.CreateNew}
            </button>
            <h2>{ScriptResources.GiftCardsList}</h2>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>{ScriptResources.GiftCardId}</th>
                        <th>{ScriptResources.ExpirationDate}</th>
                        <th>{ScriptResources.Amount}</th>
                        <th>{ScriptResources.Code}</th>
                        <th>{ScriptResources.ReceiveTime}</th>
                        <th>{ScriptResources.Actions}</th>
                    </tr>
                </thead>
                <tbody>
                    {giftCards.map((giftCard) => (
                        <tr key={giftCard.giftCardId}
                            onDoubleClick={() => handleIconClick(giftCard.giftCardId)}>
                            <td>{giftCard.giftCardId ?? '-'}</td>
                            <td>{new Date(giftCard.expirationDate).toLocaleDateString()}</td>
                            <td>{giftCard.amount.toFixed(2)}</td>
                            <td>{giftCard.code}</td>
                            <td>{new Date(giftCard.receiveTime).toLocaleString()}</td>
                            <td>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer' }}
                                    onClick={() => handleIconClick(giftCard.giftCardId)}
                                >
                                    open_in_new
                                </span>
                                <span
                                    className="material-icons"
                                    style={{ cursor: 'pointer', marginRight: '10px' }}
                                    onClick={() => handleDelete(giftCard.giftCardId)}
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

export default GiftCards;
