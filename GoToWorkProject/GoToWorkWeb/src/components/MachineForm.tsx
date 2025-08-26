import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getMachine, createMachine, updateMachine } from '../services/machineService';
import { getEmployees } from '../services/employeeService';
import { MachineType } from '../types/types';

const MachineForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [model, setModel] = useState('');
    const [type, setType] = useState(MachineType.None);
    const [selectedEmployees, setSelectedEmployees] = useState(new Set());

    const [availableEmployees, setAvailableEmployees] = useState([]);
    
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        getEmployees()
            .then(employees => {
                setAvailableEmployees(employees || []);
                if (id) {
                    return getMachine(id);
                }
                return Promise.resolve(null);
            })
            .then(machineData => {
                if (machineData) {
                    setModel(machineData.model);
                    setType(machineData.type);
                    const employeeSet = new Set((machineData.employees || []).map(e => e.employeeId));
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
        
        const machineData = {
            id,
            model,
            type,
            employees: Array.from(selectedEmployees).map(employeeId => ({ employeeId }))
        };

        try {
            if (id) {
                await updateMachine(machineData);
            } else {
                await createMachine(machineData);
            }
            navigate('/machines');
        } catch (err) {
            setError('Failed to save machine. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Machine' : 'Create Machine'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Model</Form.Label>
                    <Form.Control type="text" value={model} onChange={(e) => setModel(e.target.value)} required />
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Type</Form.Label>
                    <Form.Select value={type} onChange={(e) => setType(parseInt(e.target.value, 10))}>
                        {Object.entries(MachineType).filter(([key]) => isNaN(Number(key))).map(([key, value]) => (
                            <option key={value} value={value}>{key}</option>
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

export default MachineForm;
