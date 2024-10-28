import React from 'react';
import { Navbar, Nav, NavDropdown, Container } from "react-bootstrap";
import { Link } from 'react-router-dom';

const NavigationBar: React.FC = () => {
    return (
        <Navbar bg="dark" variant="dark" expand="lg" fixed="top">
            <Container>
                <Navbar.Brand as={Link} to="/">POS System</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/">Home</Nav.Link>
                        <Nav.Link as={Link} to="/items">Items</Nav.Link>
                        <Nav.Link as={Link} to="/inventory">Inventory</Nav.Link>
                        <Nav.Link as={Link} to="/employees">Employees</Nav.Link>
                        <Nav.Link as={Link} to="/establishments">Establishments</Nav.Link>
                        <NavDropdown title="Settings" id="basic-nav-dropdown">
                            <NavDropdown.Item as={Link} to="/settings/profile">Profile</NavDropdown.Item>
                            <NavDropdown.Item as={Link} to="/settings/preferences">Preferences</NavDropdown.Item>
                            <NavDropdown.Divider />
                            <NavDropdown.Item as={Link} to="/logout">Logout</NavDropdown.Item>
                        </NavDropdown>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default NavigationBar;
