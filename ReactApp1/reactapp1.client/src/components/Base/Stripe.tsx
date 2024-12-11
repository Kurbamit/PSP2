import React, { useState } from 'react';
import { Elements, CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import { loadStripe } from '@stripe/stripe-js';
import { Form } from 'react-bootstrap';
import axios from 'axios';
import ScriptResources from "../../assets/resources/strings.ts";
import { Order } from "../../components/Domain/Order/Orders.tsx";

interface StripePaymentProps {
    order: Order;
    token: string;
    paymentValue: number;
    setPaymentValue: React.Dispatch<React.SetStateAction<number>>; 
    handlePayPayment: () => void;
}

const StripePayment: React.FC<StripePaymentProps> = ({
    order,
    token,
    paymentValue,
    setPaymentValue,
    handlePayPayment,
}) => {

    const stripe = useStripe();
    const elements = useElements();

    const cardOptions = {
        hidePostalCode: true
    };

    const handleStripePayment = async () => {
        if (order.orderId) {
            try {
                const { data: clientSecret } = await axios.post(
                    `http://localhost:5114/api/payments/createIntent/${paymentValue}/${ScriptResources.Currency}`,
                    {},
                    { headers: { Authorization: `Bearer ${token}` } }
                );

                if (!stripe || !elements) {
                    return;
                }

                const cardElement = elements.getElement(CardElement);
                if (!cardElement) {
                    return;
                }

                const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret.clientSecret, {
                    payment_method: {
                        card: cardElement,
                    },
                });
                if (error) {
                    alert(`Payment failed: ${error.message}`);
                } else if (paymentIntent.status === 'succeeded') {
                    alert("Payment successful");
                    handlePayPayment();
                }
            } catch (error) {
                console.error("Error in Stripe payment: ", error);
                alert("Payment failed.");
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
                <button className="btn btn-secondary">
                    {ScriptResources.Cancel}
                </button>
                <button className="btn btn-primary"
                    onClick={handleStripePayment}
                    disabled={paymentValue > order.leftToPay || paymentValue <= 0}
                >
                    {ScriptResources.Pay}
                </button>
            </div>
        </div>

    );
};

export default StripePayment;
