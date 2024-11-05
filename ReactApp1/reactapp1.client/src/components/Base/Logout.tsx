import React, { useEffect } from 'react';
import { Navigate } from 'react-router-dom';

interface LogoutProps {
    onLogout: () => void;
}

const Logout: React.FC<LogoutProps> = ({ onLogout }) => {
    useEffect(() => {
        // Clear the token and update the login state
        onLogout();
    }, [onLogout]);

    // Redirect to the home page after logging out
    return <Navigate to="/" replace />;
};

export default Logout;
