import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getProduction, createProduction, updateProduction } from '../services/productionService';
import { getDetails } from '../services/detailService';

const ProductionForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [selectedDetails, setSelectedDetails] = useState(new Set());

    const [availableDetails, setAvailableDetails] = useState([]);
    
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        getDetails()
            .then(details => {
                setAvailableDetails(details || []);
                if (id) {
                    return getProduction(id);
                }
                return Promise.resolve(null);
            })
            .then(productionData => {
                if (productionData) {
                    setName(productionData.name);
                    const detailsSet = new Set((productionData.details || []).map(d => d.detailId));
                    setSelectedDetails(detailsSet);
                }
            })
            .catch(() => setError('Failed to fetch initial data'))
            .finally(() => setIsLoading(false));
    }, [id]);

    const handleDetailChange = (detailId) => {
        const newSelection = new Set(selectedDetails);
        if (newSelection.has(detailId)) {
            newSelection.delete(detailId);
        } else {
            newSelection.add(detailId);
        }
        setSelectedDetails(newSelection);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        
        const productionData = {
            id,
            name,
            details: Array.from(selectedDetails).map(detailId => ({ detailId }))
        };

        try {
            if (id) {
                await updateProduction(productionData);
            } else {
                await createProduction(productionData);
            }
            navigate('/productions');
        } catch (err) {
            setError('Failed to save production. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Production' : 'Create Production'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Name</Form.Label>
                    <Form.Control type="text" value={name} onChange={(e) => setName(e.target.value)} required />
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Details</Form.Label>
                    {availableDetails.map(detail => (
                        <Form.Check 
                            key={detail.id}
                            type="checkbox"
                            label={detail.name}
                            checked={selectedDetails.has(detail.id)}
                            onChange={() => handleDetailChange(detail.id)}
                        />
                    ))}
                </Form.Group>

                <Button variant="primary" type="submit">Save</Button>
            </Form>
        </Container>
    );
};

export default ProductionForm;
