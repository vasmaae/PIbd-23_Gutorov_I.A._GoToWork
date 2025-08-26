import { useState } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { register } from '../services/authService';

const RegisterPage = () => {
    const [login, setLogin] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState(1); // Default to Executor
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');
        try {
            await register(login, email, password, role);
            setSuccess('Registration successful! You can now log in.');
        } catch (err) {
            setError('Registration failed. Please try again.');
        }
    };

    return (
        <Container className="mt-5 auth-container">
            <h2>Register</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            {success && <Alert variant="success">{success}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Login</Form.Label>
                    <Form.Control type="text" value={login} onChange={(e) => setLogin(e.target.value)} required />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Email</Form.Label>
                    <Form.Control type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Password</Form.Label>
                    <Form.Control type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Role</Form.Label>
                    <Form.Select value={role} onChange={(e) => setRole(parseInt(e.target.value, 10))}>
                        <option value={1}>Executor</option>
                        <option value={2}>Guarantor</option>
                    </Form.Select>
                </Form.Group>
                <Button variant="primary" type="submit">Register</Button>
            </Form>
        </Container>
    );
};

export default RegisterPage;
