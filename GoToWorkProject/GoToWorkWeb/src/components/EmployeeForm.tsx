import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getEmployee, createEmployee, updateEmployee } from '../services/employeeService';

const EmployeeForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [fullName, setFullName] = useState('');
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        if (id) {
            setIsLoading(true);
            getEmployee(id)
                .then(data => {
                    setFullName(data.fullName);
                })
                .catch(() => setError('Failed to fetch employee'))
                .finally(() => setIsLoading(false));
        }
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        const employeeData = { id, fullName };

        try {
            if (id) {
                await updateEmployee(employeeData);
            } else {
                await createEmployee(employeeData);
            }
            navigate('/employees');
        } catch (err) {
            setError('Failed to save employee. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Employee' : 'Create Employee'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Full Name</Form.Label>
                    <Form.Control type="text" value={fullName} onChange={(e) => setFullName(e.target.value)} required />
                </Form.Group>
                <Button variant="primary" type="submit">Save</Button>
            </Form>
        </Container>
    );
};

export default EmployeeForm;
