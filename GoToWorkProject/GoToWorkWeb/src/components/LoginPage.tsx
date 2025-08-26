import {useState} from 'react';
import {Form, Button, Container, Alert} from 'react-bootstrap';
import {login} from '../services/authService';
import {useNavigate} from 'react-router-dom';

const LoginPage = () => {
    const [loginField, setLoginField] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await login(loginField, password);
            window.dispatchEvent(new Event('authChange'));
            navigate('/');
        } catch (err) {
            setError('Login failed. Please check your credentials.');
        }
    };

    return (
        <Container className="mt-5 auth-container">
            <h2>Login</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Login</Form.Label>
                    <Form.Control type="text" value={loginField} onChange={(e) => setLoginField(e.target.value)}
                                  required/>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Password</Form.Label>
                    <Form.Control type="password" value={password} onChange={(e) => setPassword(e.target.value)}
                                  required/>
                </Form.Group>
                <Button variant="primary" type="submit">Login</Button>
            </Form>
        </Container>
    );
};

export default LoginPage;
