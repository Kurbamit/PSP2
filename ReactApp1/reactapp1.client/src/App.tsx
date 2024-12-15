import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from "./components/Base/Navbar.tsx";
import Items from "./components/Domain/Item/Items.tsx";
import ItemDetail from "./components/Domain/Item/ItemDetail.tsx";
import Register from "./components/Base/Register.tsx";
import Login from "./components/Base/Login.tsx";
import Logout from "./components/Base/Logout.tsx";
import './App.css';
import Cookies from 'js-cookie';
import Employees from "./components/Domain/Employee/Employees.tsx";
import EmployeeDetail from "./components/Domain/Employee/EmployeeDetail.tsx";
import Orders from "./components/Domain/Order/Orders.tsx";
import OrderDetail from "./components/Domain/Order/OrderDetail.tsx";
import Reservations from './components/Domain/Reservation/Reservations.tsx';
import ReservationDetail from './components/Domain/Reservation/ReservationDetail.tsx';
import { ErrorProvider } from "./components/Base/ErrorContext.tsx";
import useAxiosInterceptors from "./assets/Utils/axiosInterceptor.ts";
import GlobalAlert from "./components/Base/GlobalAlert.tsx";
import Services from './components/Domain/Service/Services.tsx';
import ServiceDetail from './components/Domain/Service/ServiceDetail.tsx';
import Receipt from "./components/Domain/Order/Receipt.tsx";
import Giftcards from './components/Domain/Giftcard/Giftcards.tsx'
import GiftcardDetail from './components/Domain/Giftcard/GiftcardDetail.tsx'

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const token = Cookies.get('authToken');
        if (token) {
            setIsLoggedIn(true);
        }
    }, []);

    const handleLogin = (token: string) => {
        Cookies.set('authToken', token, { expires: 1 });
        setIsLoggedIn(true);
    };

    const handleLogout = () => {
        Cookies.remove('authToken');
        setIsLoggedIn(false);
    };

    const IndexPage = () => (
        <div>
            <h1>Welcome to the POS System</h1>
            {isLoggedIn ? (
                <p>You are logged in. You can now access the contents.</p>
            ) : (
            <p>Login in order to see contents.</p>)}
        </div>
    );

    return (
        <ErrorProvider>
            <InnerApp
                contents={IndexPage()}
                isLoggedIn={isLoggedIn}
                handleLogin={handleLogin}
                handleLogout={handleLogout}
            />
        </ErrorProvider>
    );
}

function InnerApp({
                      contents,
                      isLoggedIn,
                      handleLogin,
                      handleLogout,
                  }: {
    contents: React.ReactNode;
    isLoggedIn: boolean;
    handleLogin: (token: string) => void;
    handleLogout: () => void;
}) {
    useAxiosInterceptors();

    return (
        <Router>
            <NavigationBar isLoggedIn={isLoggedIn} />
            <GlobalAlert />
            <div style={{ marginTop: '4rem', padding: '1rem' }}>
                <Routes>
                    <Route path="/" element={
                        <div>
                            {contents}
                        </div>
                    } />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/orders/:id" element={<OrderDetail />} />
                    <Route path="/reservations" element={<Reservations />} />
                    <Route path="/reservations/new" element={<ReservationDetail /> } />
                    <Route path="/reservations/:id" element={<ReservationDetail />} />
                    <Route path="/giftcards" element={<Giftcards />} />
                    <Route path="/giftcards/new" element={<GiftcardDetail />} />
                    <Route path="/giftcards/:id" element={<GiftcardDetail />} />
                    <Route path="/receipt/:id" element={<Receipt />} />
                    <Route path="/items" element={<Items />} />
                    <Route path="/items/new" element={<ItemDetail />} />
                    <Route path="/services" element={<Services />} />
                    <Route path="/services/new" element={<ServiceDetail />} />
                    <Route path="/services/:id" element={<ServiceDetail />} />
                    <Route path="/items/:id" element={<ItemDetail />} />
                    <Route path="/register" element={<Register onRegister={handleLogin} />} />
                    <Route path="/employees" element={<Employees />} />
                    <Route path="/employees/new" element={<EmployeeDetail />} />
                    <Route path="/employees/:id" element={<EmployeeDetail />} />
                    <Route path="/login" element={<Login onLogin={handleLogin} />} />
                    <Route path="/logout" element={<Logout onLogout={handleLogout} />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
