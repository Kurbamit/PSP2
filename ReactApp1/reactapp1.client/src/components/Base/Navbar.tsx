import React, { useState, useEffect } from 'react';
import { Navbar, Nav, NavDropdown, Container } from "react-bootstrap";
import { Link } from 'react-router-dom';
import ScriptResources from "../../assets/resources/strings.ts";
import axios from 'axios';
import {API_BASE_URL} from "../../../config.ts";
import Cookies from "js-cookie";

interface NavigationBarProps {
    isLoggedIn: boolean;
}

interface NavigationItem {
    path: string;
    label: string;
    isDropdown?: boolean;
    dropdownItems?: Array<{ path: string; label: string }>;
}

const NavigationBar: React.FC<NavigationBarProps> = ({ isLoggedIn }) => {
    const token = Cookies.get('authToken');
    const [navItems, setNavItems] = useState<NavigationItem[]>([]);

    useEffect(() => {
        const fetchNavigation = async () => {
            if (isLoggedIn) {
                try {
                    const response = await axios.get(`${API_BASE_URL}/navigation`, {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    console.log("Navigation response:", response.data);
                    setNavItems(response.data.items || []);  // Ensure it uses an array or defaults to empty
                } catch (error) {
                    console.error("Error fetching navigation items:", error);
                }
            } else {
                setNavItems([
                    { path: '/register', label: ScriptResources.Register },
                    { path: '/login', label: ScriptResources.Login }
                ]);
            }
        };

        fetchNavigation();
    }, [isLoggedIn]);

    return (
        <Navbar bg="dark" variant="dark" expand="lg" fixed="top">
            <Container>
                <Navbar.Brand as={Link} to="/">{ScriptResources.POSSystem}</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/">{ScriptResources.Home}</Nav.Link>
                        {navItems.map((item, index) => (
                            item.isDropdown ? (
                                <NavDropdown key={index} title={item.label} id={`nav-dropdown-${index}`}>
                                    {item.dropdownItems && item.dropdownItems.map((subItem, subIndex) => (
                                        <NavDropdown.Item as={Link} key={subIndex} to={subItem.path}>
                                            {subItem.label}
                                        </NavDropdown.Item>
                                    ))}
                                </NavDropdown>
                            ) : (
                                <Nav.Link as={Link} key={index} to={item.path ?? '#'}>
                                    {item.label}
                                </Nav.Link>
                            )
                        ))}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );

};

export default NavigationBar;
