import { useState, useEffect } from 'react';
import { Container, Row, Col, Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getToken, getUser } from '../services/authService';

import DetailsIcon from './icons/DetailsIcon';
import ProductsIcon from './icons/ProductsIcon';
import ProductionsIcon from './icons/ProductionsIcon';
import ReportsIcon from './icons/ReportsIcon';
import EmployeesIcon from './icons/EmployeesIcon';
import MachinesIcon from './icons/MachinesIcon';
import WorkshopsIcon from './icons/WorkshopsIcon';

const HomePage = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
        const updateAuthState = () => {
            setIsAuthenticated(!!getToken());
            setUser(getUser());
        };

        updateAuthState();
        window.addEventListener('authChange', updateAuthState);

        return () => {
            window.removeEventListener('authChange', updateAuthState);
        };
    }, []);

    const renderGuestHomepage = () => (
        <div className="p-5 mb-4 bg-light rounded-3">
            <Container fluid className="py-5">
                <h1 className="display-5 fw-bold">Добро пожаловать на завод "Иди работать"!</h1>
                <p className="col-md-8 fs-4">
                    Эта система предназначена для комплексного управления производственными процессами, от учета деталей до контроля над цехами и станками.
                </p>
                <p>Для начала работы войдите в систему или зарегистрируйтесь.</p>
                <Link to="/login" className="btn btn-primary btn-lg me-2">Вход</Link>
                <Link to="/register" className="btn btn-secondary btn-lg">Регистрация</Link>
            </Container>
        </div>
    );

    const renderAuthenticatedHomepage = () => {
        const userRole = user?.role;
        const executorLinks = [
            { to: '/details', text: 'Детали', description: 'Управление списком деталей.', icon: <DetailsIcon /> },
            { to: '/products', text: 'Изделия', description: 'Просмотр и управление изделиями.', icon: <ProductsIcon /> },
            { to: '/productions', text: 'Производства', description: 'Обзор производственных линий.', icon: <ProductionsIcon /> },
            { to: '/reports', text: 'Отчёты', description: 'Генерация и просмотр отчетов.', icon: <ReportsIcon /> },
        ];

        const guarantorLinks = [
            { to: '/employees', text: 'Работники', description: 'Управление персоналом.', icon: <EmployeesIcon /> },
            { to: '/machines', text: 'Станки', description: 'Учет и управление оборудованием.', icon: <MachinesIcon /> },
            { to: '/workshops', text: 'Цеха', description: 'Обзор и управление цехами.', icon: <WorkshopsIcon /> },
            { to: '/reports', text: 'Отчёты', description: 'Генерация и просмотр отчетов.', icon: <ReportsIcon /> },
        ];

        const links = userRole === 1 ? executorLinks : guarantorLinks;

        return (
            <Container className="mt-5">
                 <div className="p-5 mb-4 bg-light rounded-3">
                    <h1 className="display-5 fw-bold">С возвращением, {user?.login}!</h1>
                    <p className="lead">Выберите раздел для начала работы.</p>
                </div>
                <Row>
                    {links.map((link, index) => (
                        <Col md={4} lg={3} key={index} className="mb-4">
                            <Card className="h-100 dashboard-card">
                                <Card.Body className="d-flex flex-column text-center">
                                    <div className="mb-3">{link.icon}</div>
                                    <Card.Title className="mb-2">{link.text}</Card.Title>
                                    <Card.Text className="text-muted flex-grow-1">{link.description}</Card.Text>
                                    <Link to={link.to} className="btn btn-primary mt-auto">Перейти</Link>
                                </Card.Body>
                            </Card>
                        </Col>
                    ))}
                </Row>
            </Container>
        );
    };

    return isAuthenticated ? renderAuthenticatedHomepage() : renderGuestHomepage();
};

export default HomePage;
