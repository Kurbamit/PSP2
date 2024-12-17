import axios from 'axios';
import { useError } from '../../components/Base/ErrorContext';

const useAxiosInterceptors = () => {
    const { setError } = useError();

    axios.interceptors.response.use(
        (response) => response, // Pass through successful responses
        (error) => {

            if (error.response) {
                // Response exists, handle based on status code
                if (error.response.status === 401) {
                    setError("You are not authorized to perform this action. Please log in and try again.");
                } else {
                    // Handle other server errors
                    const message =
                        error.response.data?.Title ||
                        error.response.data?.message ||
                        "An error occurred."; // Default message if none is found
                    setError(message);
                }
            } else {
                // No response, likely a network error
                console.error("Error details:", error); // Debugging network errors
                setError("A network error occurred. Please try again.");
            }
            return Promise.reject(error); // Reject the promise so further handling can occur
        }
    );
};

export default useAxiosInterceptors;
