import { Container, Nav, Navbar, Button } from 'react-bootstrap';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { getToken, logout, getUserRole } from '../services/authService';
import { useEffect, useState } from 'react';
import Footer from './Footer';

const MainLayout = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(!!getToken());
    const [userRole, setUserRole] = useState(getUserRole());
    const navigate = useNavigate();

    useEffect(() => {
        const handleAuthChange = () => {
            setIsAuthenticated(!!getToken());
            setUserRole(getUserRole());
        };
        window.addEventListener('authChange', handleAuthChange);
        window.addEventListener('storage', handleAuthChange);
        return () => {
            window.removeEventListener('authChange', handleAuthChange);
            window.removeEventListener('storage', handleAuthChange);
        };
    }, []);

    const handleLogout = () => {
        logout();
        window.dispatchEvent(new Event('authChange'));
        navigate('/login');
    };

    const renderExecutorLinks = () => (
        <>
            <Nav.Link as={Link} to="/details">Детали</Nav.Link>
            <Nav.Link as={Link} to="/products">Изделия</Nav.Link>
            <Nav.Link as={Link} to="/productions">Производства</Nav.Link>
            <Nav.Link as={Link} to="/reports">Отчёты</Nav.Link>
        </>
    );

    const renderGuarantorLinks = () => (
        <>
            <Nav.Link as={Link} to="/employees">Работники</Nav.Link>
            <Nav.Link as={Link} to="/machines">Станки</Nav.Link>
            <Nav.Link as={Link} to="/workshops">Цеха</Nav.Link>
            <Nav.Link as={Link} to="/reports">Отчёты</Nav.Link>
        </>
    );

    return (
        <div style={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
            <Navbar bg="dark" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand as={Link} to="/">Завод "Иди работать"</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            {isAuthenticated && userRole === 1 && renderExecutorLinks()}
                            {isAuthenticated && userRole === 2 && renderGuarantorLinks()}
                        </Nav>
                        <Nav>
                            {!isAuthenticated ? (
                                <>
                                    <Nav.Link as={Link} to="/login">Вход</Nav.Link>
                                    <Nav.Link as={Link} to="/register">Регистрация</Nav.Link>
                                </>
                            ) : (
                                <Button variant="outline-light" onClick={handleLogout}>Выход</Button>
                            )}
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container className="mt-4" style={{ flex: 1 }}>
                <Outlet />
            </Container>
            <Footer />
        </div>
    );
};

export default MainLayout;
