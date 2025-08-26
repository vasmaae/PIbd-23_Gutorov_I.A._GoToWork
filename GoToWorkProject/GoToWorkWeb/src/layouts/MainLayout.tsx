import { Container, Nav, Navbar } from 'react-bootstrap';
import { Link, Outlet } from 'react-router-dom';

const MainLayout = () => {
    return (
        <>
            <Navbar bg="dark" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand as={Link} to="/">GoToWork</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link as={Link} to="/">Home</Nav.Link>
                            <Nav.Link as={Link} to="/login">Login</Nav.Link>
                            <Nav.Link as={Link} to="/register">Register</Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container className="mt-4">
                <Outlet />
            </Container>
        </>
    );
};

export default MainLayout;
