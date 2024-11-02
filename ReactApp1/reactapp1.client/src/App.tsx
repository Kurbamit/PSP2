import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from "./components/Base/Navbar.tsx";
import Items from "./components/Domain/Items.tsx";
import './App.css';
import { API_BASE_URL } from "../config";

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    const [forecasts, setForecasts] = useState<Forecast[]>();

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
            <NavigationBar />
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
                </Routes>
            </div>
        </Router>
    );
}

export default App;
