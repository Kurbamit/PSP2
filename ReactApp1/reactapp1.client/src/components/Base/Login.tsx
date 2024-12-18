import React, { useState } from 'react';
import { Form, Button, Container, Row, Col, Alert } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { API_BASE_URL } from "../../../config.ts";
import ScriptResources from "../../assets/resources/strings.ts";

interface LoginProps {
    onLogin: (token: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [alertMessage, setAlertMessage] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setAlertMessage(null); // Clear previous alert messages

        const response = await fetch(`${API_BASE_URL}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            const data = await response.json();
            const { token } = data;
            console.log(ScriptResources.LoginSuccessful);
            onLogin(token);
            navigate('/');
        } else {
            setAlertMessage(ScriptResources.LoginFailed + response.statusText);
            console.log(ScriptResources.LoginFailed);
        }
    };

    return (
        <Container className="mt-5">
            <Row className="justify-content-md-center">
                <Col md={12}>
                    <h2 className="text-center">{ScriptResources.Login}</h2>

                    {/* Display Bootstrap alert if there is an alert message */}
                    {alertMessage && (
                        <Alert variant="danger" onClose={() => setAlertMessage(null)} dismissible>
                            <strong>{ScriptResources.Error}</strong> {alertMessage}
                        </Alert>
                    )}

                    <Form onSubmit={handleLogin}>
                        <Form.Group className="mb-3" controlId="formEmail">
                            <Form.Label>{ScriptResources.Email}</Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="Enter email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formPassword">
                            <Form.Label>{ScriptResources.Password}</Form.Label>
                            <Form.Control
                                type="password"
                                placeholder="Password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </Form.Group>

                        <Button variant="primary" type="submit" className="w-100">
                            {ScriptResources.Login}
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};

export default Login;
