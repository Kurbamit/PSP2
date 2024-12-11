import React, { useEffect } from 'react';
import { useError } from './ErrorContext';

const GlobalAlert: React.FC = () => {
    const { error, setError } = useError();

    // Log the error state
    console.log('GlobalAlert error:', error);

    // Ensure alert is shown
    useEffect(() => {
        if (error) {
            const timeout = setTimeout(() => {
                setError(null); // Reset error after a few seconds
            }, 5000);

            return () => clearTimeout(timeout);
        }
    }, [error]);

    if (!error) return null;

    return (
        <div className="alert alert-danger alert-dismissible fade show" role="alert">
            {error}
            <button
                type="button"
                className="btn-close"
                aria-label="Close"
                onClick={() => setError(null)} // Close the alert
            ></button>
        </div>
    );
};

export default GlobalAlert;
