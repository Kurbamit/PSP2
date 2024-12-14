import React, { useState } from 'react';
import { CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import { Form } from 'react-bootstrap';
import axios from 'axios';
import ScriptResources from "../../../assets/resources/strings.ts";
import { Order } from "../../../components/Domain/Order/Orders.tsx";

interface StripePaymentProps {
    order: Order | undefined;
    token: string | undefined;
    paymentValue: number;
    setShowPaymentModal: React.Dispatch<React.SetStateAction<boolean>>;
    setPaymentValue: React.Dispatch<React.SetStateAction<number>>; 
    handlePayPayment: (stripePaymentId: string) => void;
}

const StripePayment: React.FC<StripePaymentProps> = ({
    order,
    token,
    paymentValue,
    setPaymentValue,
    setShowPaymentModal,
    handlePayPayment,
}) => {

    const stripe = useStripe();
    const elements = useElements();
    const [isProcessing, setIsProcessing] = useState(false);

    const cardOptions = {
        hidePostalCode: true
    };

    const handleStripePayment = async () => {
        if (order?.orderId) {
            try {
                setIsProcessing(true);

                if (!stripe || !elements) {
                    setIsProcessing(false);
                    alert(`Stripe initialization failed`);
                    return;
                }

                const cardElement = elements.getElement(CardElement);
                if (!cardElement) {
                    setIsProcessing(false);
                    alert(`Payment initialization failed`);
                    return;
                }

                const { data: clientSecret } = await axios.post(
                    `http://localhost:5114/api/payments/createIntent/${paymentValue}/${ScriptResources.Currency}`,
                    {},
                    { headers: { Authorization: `Bearer ${token}` } }
                );

                const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret.clientSecret, {
                    payment_method: {
                        card: cardElement,
                    },
                });

                if (error) {
                    alert(`Payment failed: ${error.message}`);
                } else if (paymentIntent.status === 'succeeded') {
                    alert("Payment successful");
                    handlePayPayment(paymentIntent.id);
                }

            } catch (error) {
                console.error("Error in Stripe payment: ", error);
            } finally {
                setIsProcessing(false);
            }
        }
    };

    return (
        <div>
            <CardElement options={cardOptions} />

            <Form.Group className="mb-3" controlId="payment-amount">
                <Form.Label>{ScriptResources.Amount}</Form.Label>
                <Form.Control
                    type="number"
                    value={paymentValue}
                    onChange={(e) => {
                        let value = e.target.value;
                        if (/^\d*\.?\d{0,2}$/.test(value)) { // limit to 2 decimal places
                            setPaymentValue(parseFloat(value));
                        }
                    }}
                    min="0"
                />
            </Form.Group>
            <div className="modal-footer">
                <button className="btn btn-secondary"
                    onClick={() => setShowPaymentModal(false)}>
                    {ScriptResources.Cancel}
                </button>
                <button className="btn btn-primary"
                    onClick={handleStripePayment}
                    disabled={isProcessing || paymentValue > (order?.leftToPay ?? 0) || paymentValue <= 0}
                >
                    {isProcessing? ScriptResources.Processing : ScriptResources.Pay}
                </button>
            </div>
        </div>

    );
};

export default StripePayment;
