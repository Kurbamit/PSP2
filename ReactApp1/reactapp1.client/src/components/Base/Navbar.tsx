import React from 'react';
import { Navbar, Nav, NavDropdown, Container } from "react-bootstrap";
import { Link } from 'react-router-dom';
import ScriptResources from "../../assets/resources/strings.ts";

interface NavigationBarProps {
    isLoggedIn: boolean;
}

const NavigationBar: React.FC<NavigationBarProps> = ({ isLoggedIn }) => {
    return (
        <Navbar bg="dark" variant="dark" expand="lg" fixed="top">
            <Container>
                <Navbar.Brand as={Link} to="/">{ScriptResources.POSSystem}</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/">{ScriptResources.Home}</Nav.Link>
                        {isLoggedIn && (
                            <>
                                <Nav.Link as={Link} to="/items">{ScriptResources.Items}</Nav.Link>
                                <Nav.Link as={Link} to="/inventory">{ScriptResources.Inventory}</Nav.Link>
                                <Nav.Link as={Link} to="/employees">{ScriptResources.Employees}</Nav.Link>
                                <Nav.Link as={Link} to="/establishments">{ScriptResources.Establishments}</Nav.Link>
                            </>
                        )}
                        {isLoggedIn ? (
                            <NavDropdown title="Settings" id="basic-nav-dropdown">
                                <NavDropdown.Item as={Link} to="/settings/profile">{ScriptResources.Profile}</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to="/settings/preferences">{ScriptResources.Preferences}</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item as={Link} to="/logout">{ScriptResources.Logout}</NavDropdown.Item>
                            </NavDropdown>
                        ) : (
                            <>
                                <Nav.Link as={Link} to="/register">{ScriptResources.Register}</Nav.Link>
                                <Nav.Link as={Link} to="/login">{ScriptResources.Login}</Nav.Link>
                            </>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default NavigationBar;
