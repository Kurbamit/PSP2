import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from "./components/Base/Navbar.tsx";
import Items from "./components/Domain/Items.tsx";
import Register from "./components/Base/Register.tsx";
import Login from "./components/Base/Login.tsx";
import Logout from "./components/Base/Logout.tsx";
import './App.css';
import { API_BASE_URL } from "../config";

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [forecasts, setForecasts] = useState<Forecast[]>();

    const handleLogin = () => setIsLoggedIn(true);  // Mock login
    const handleLogout = () => setIsLoggedIn(false); // Mock logout

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
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

    async function populateWeatherData() {
        const response = await fetch(`${API_BASE_URL}/weatherforecast`);
        const data = await response.json();
        setForecasts(data);
    }

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
                    <Route path="/register" element={<Register onRegister={handleLogin} />} />
                    <Route path="/login" element={<Login onLogin={handleLogin} />} />
                    <Route path="/logout" element={
                        <Logout onLogout={handleLogout} />
                    } />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
