import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from "./components/Base/Navbar.tsx";
import Items from "./components/Domain/Item/Items.tsx";
import ItemDetail from "./components/Domain/Item/ItemDetail.tsx";
import Register from "./components/Base/Register.tsx";
import Login from "./components/Base/Login.tsx";
import Logout from "./components/Base/Logout.tsx";
import './App.css';
import { API_BASE_URL } from "../config";
import Cookies from 'js-cookie';
import Employees from "./components/Domain/Employee/Employees.tsx";
import EmployeeDetail from "./components/Domain/Employee/EmployeeDetail.tsx";

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    const [forecasts, setForecasts] = useState<Forecast[]>();
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        // Check if authToken is present in cookies
        const token = Cookies.get('authToken');
        if (token) {
            setIsLoggedIn(true); // Set logged in status to true if token exists
            populateWeatherData();
        }
    }, []);

    const handleLogin = (token: string) => {
        Cookies.set('authToken', token, { expires: 1 }); // Save token in cookies, expires in 7 days
        setIsLoggedIn(true);
        populateWeatherData();
    };

    const handleLogout = () => {
        Cookies.remove('authToken'); // Remove token from cookies
        setIsLoggedIn(false);
    };

    const contents = forecasts === undefined
        ? <div>
            <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
            <p>Error might be because You're not logged in.</p>
        </div>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
            <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
            </tr>
            </thead>
            <tbody>
            {forecasts.map(forecast =>
                <tr key={forecast.date}>
                    <td>{forecast.date}</td>
                    <td>{forecast.temperatureC}</td>
                    <td>{forecast.temperatureF}</td>
                    <td>{forecast.summary}</td>
                </tr>
            )}
            </tbody>
        </table>;

    return (
        <Router>
            <NavigationBar isLoggedIn={isLoggedIn} />
            <div style={{ marginTop: '4rem', padding: '1rem' }}>
                <Routes>
                    <Route path="/" element={
                        <div>
                            <h1 id="tabelLabel">Weather forecast</h1>
                            <p>This component demonstrates fetching data from the server.</p>
                            {contents}
                        </div>
                    } />
                    <Route path="/items" element={<Items />} />
                    <Route path="/items/new" element={<ItemDetail />} />
                    <Route path="/items/:id" element={<ItemDetail />} />
                    <Route path="/register" element={<Register onRegister={handleLogin} />} />
                    <Route path="/employees" element={<Employees />} />
                    <Route path="/employees/new" element={<EmployeeDetail />} />
                    <Route path="/employees/:id" element={<EmployeeDetail />} />
                    <Route path="/login" element={<Login onLogin={handleLogin} />} />
                    <Route path="/logout" element={
                        <Logout onLogout={handleLogout} />
                    } />
                </Routes>
            </div>
        </Router>
    );

    async function populateWeatherData() {
        const token = Cookies.get('authToken');
        const response = await fetch(`${API_BASE_URL}/GetWeatherForecast`,
            {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
        const data = await response.json();
        setForecasts(data);
    }
}

export default App;
