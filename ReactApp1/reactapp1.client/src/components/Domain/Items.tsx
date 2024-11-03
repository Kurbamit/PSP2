// src/components/Items.tsx
import React from 'react';
import Cookies from "js-cookie";

const Items: React.FC = () => {
    const token = Cookies.get('token');
    return (
        <div>
            <h2>Items List</h2>
            <p>This is where the items will be displayed.</p>
            {token ? (
                <p>Your token: {token}</p> // Display the token if it exists
            ) : (
                <p>No token found. Please log in.</p> // Message if no token exists
            )}
        </div>
    );
};

export default Items;