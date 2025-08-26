import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getWorkshop, createWorkshop, updateWorkshop } from '../services/workshopService';
import { getEmployees } from '../services/employeeService';
import { getProductions } from '../services/productionService';

const WorkshopForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [address, setAddress] = useState('');
    const [productionId, setProductionId] = useState('');
    const [selectedEmployees, setSelectedEmployees] = useState(new Set());

    const [availableEmployees, setAvailableEmployees] = useState([]);
    const [availableProductions, setAvailableProductions] = useState([]);
    
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        Promise.all([getEmployees(), getProductions()])
            .then(([employees, productions]) => {
                setAvailableEmployees(employees || []);
                setAvailableProductions(productions || []);
                if (id) {
                    return getWorkshop(id);
                }
                return Promise.resolve(null);
            })
            .then(workshopData => {
                if (workshopData) {
                    setAddress(workshopData.address);
                    setProductionId(workshopData.productionId || '');
                    const employeeSet = new Set((workshopData.employees || []).map(e => e.employeeId));
                    setSelectedEmployees(employeeSet);
                }
            })
            .catch(() => setError('Failed to fetch initial data'))
            .finally(() => setIsLoading(false));
    }, [id]);

    const handleEmployeeChange = (employeeId) => {
        const newSelection = new Set(selectedEmployees);
        if (newSelection.has(employeeId)) {
            newSelection.delete(employeeId);
        } else {
            newSelection.add(employeeId);
        }
        setSelectedEmployees(newSelection);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        
        const workshopData = {
            id,
            address,
            productionId: productionId || null,
            employees: Array.from(selectedEmployees).map(employeeId => ({ employeeId }))
        };

        try {
            if (id) {
                await updateWorkshop(workshopData);
            } else {
                await createWorkshop(workshopData);
            }
            navigate('/workshops');
        } catch (err) {
            setError('Failed to save workshop. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Workshop' : 'Create Workshop'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Address</Form.Label>
                    <Form.Control type="text" value={address} onChange={(e) => setAddress(e.target.value)} required />
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Production</Form.Label>
                    <Form.Select value={productionId} onChange={(e) => setProductionId(e.target.value)}>
                        <option value="">Select a production</option>
                        {availableProductions.map(production => (
                            <option key={production.id} value={production.id}>{production.name}</option>
                        ))}
                    </Form.Select>
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Employees</Form.Label>
                    {availableEmployees.map(employee => (
                        <Form.Check 
                            key={employee.id}
                            type="checkbox"
                            label={employee.fullName}
                            checked={selectedEmployees.has(employee.id)}
                            onChange={() => handleEmployeeChange(employee.id)}
                        />
                    ))}
                </Form.Group>

                <Button variant="primary" type="submit">Save</Button>
            </Form>
        </Container>
    );
};

export default WorkshopForm;
