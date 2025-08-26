import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert, Row, Col } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getProduct, createProduct, updateProduct } from '../services/productService';
import { getDetails } from '../services/detailService';
import { getMachines } from '../services/machineService';

const ProductForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [machineId, setMachineId] = useState('');
    const [productDetails, setProductDetails] = useState({});

    const [availableDetails, setAvailableDetails] = useState([]);
    const [availableMachines, setAvailableMachines] = useState([]);
    
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        Promise.all([getDetails(), getMachines()])
            .then(([details, machines]) => {
                setAvailableDetails(details || []);
                setAvailableMachines(machines || []);
                if (id) {
                    return getProduct(id);
                }
                return Promise.resolve(null);
            })
            .then(productData => {
                if (productData) {
                    setName(productData.name);
                    setMachineId(productData.machineId || '');
                    const detailsMap = (productData.details || []).reduce((acc, item) => {
                        acc[item.detailId] = item.quantity;
                        return acc;
                    }, {});
                    setProductDetails(detailsMap);
                }
            })
            .catch(() => setError('Failed to fetch initial data'))
            .finally(() => setIsLoading(false));
    }, [id]);

    const handleDetailChange = (detailId, quantity) => {
        const newDetails = { ...productDetails };
        if (quantity > 0) {
            newDetails[detailId] = quantity;
        } else {
            delete newDetails[detailId];
        }
        setProductDetails(newDetails);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        
        const productData = {
            id,
            name,
            machineId: machineId || null,
            creationDate: new Date(),
            details: Object.entries(productDetails).map(([detailId, quantity]) => ({ detailId, quantity }))
        };

        try {
            if (id) {
                await updateProduct(productData);
            } else {
                await createProduct(productData);
            }
            navigate('/products');
        } catch (err) {
            setError('Failed to save product. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Product' : 'Create Product'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Name</Form.Label>
                    <Form.Control type="text" value={name} onChange={(e) => setName(e.target.value)} required />
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Machine</Form.Label>
                    <Form.Select value={machineId} onChange={(e) => setMachineId(e.target.value)}>
                        <option value="">Select a machine</option>
                        {availableMachines.map(machine => (
                            <option key={machine.id} value={machine.id}>{machine.name}</option>
                        ))}
                    </Form.Select>
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Details</Form.Label>
                    {availableDetails.map(detail => (
                        <Row key={detail.id} className="mb-2 align-items-center">
                            <Col xs={6}>
                                <Form.Check 
                                    type="checkbox"
                                    label={detail.name}
                                    checked={!!productDetails[detail.id]}
                                    onChange={e => {
                                        const newQuantity = e.target.checked ? 1 : 0;
                                        handleDetailChange(detail.id, newQuantity);
                                    }}
                                />
                            </Col>
                            <Col xs={6}>
                                <Form.Control 
                                    type="number"
                                    placeholder="Quantity"
                                    value={productDetails[detail.id] || ''}
                                    onChange={e => {
                                        const newQuantity = parseInt(e.target.value, 10) || 0;
                                        handleDetailChange(detail.id, newQuantity);
                                    }}
                                    disabled={!productDetails[detail.id]}
                                />
                            </Col>
                        </Row>
                    ))}
                </Form.Group>

                <Button variant="primary" type="submit">Save</Button>
            </Form>
        </Container>
    );
};

export default ProductForm;
