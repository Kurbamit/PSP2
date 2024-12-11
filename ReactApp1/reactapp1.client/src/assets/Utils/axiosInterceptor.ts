import axios from 'axios';
import { useError } from '../../components/Base/ErrorContext';

const useAxiosInterceptors = () => {
    const { setError } = useError();

    axios.interceptors.response.use(
        (response) => response, // Pass through successful responses
        (error) => {

            if (error.response) {
                // Check if the response contains the expected error data
                const message = error.response.data?.Title ||
                    error.response.data?.message ||
                    "An error occurred."; // Default message if none is found
                setError(message); // Set the error message in the context
            } else {
                // Handle network errors
                setError("A network error occurred. Please try again.");
            }
            return Promise.reject(error); // Reject the promise so further handling can occur
        }
    );
};

export default useAxiosInterceptors;
