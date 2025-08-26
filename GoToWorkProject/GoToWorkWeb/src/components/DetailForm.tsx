import { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import { getDetail, createDetail, updateDetail } from '../services/detailService';

const materialTypes = {
    1: 'Steel',
    2: 'Aluminum',
    3: 'Plastic',
    4: 'Copper',
    5: 'Composite'
};

const DetailForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [material, setMaterial] = useState(1);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        if (id) {
            setIsLoading(true);
            getDetail(id)
                .then(data => {
                    setName(data.name);
                    setMaterial(data.material);
                })
                .catch(() => setError('Failed to fetch detail'))
                .finally(() => setIsLoading(false));
        }
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        const detailData = { id, name, material, creationDate: new Date() };

        try {
            if (id) {
                await updateDetail(detailData);
            } else {
                await createDetail(detailData);
            }
            navigate('/details');
        } catch (err) {
            setError('Failed to save detail. Please try again.');
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container className="mt-5">
            <h2>{id ? 'Edit Detail' : 'Create Detail'}</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Name</Form.Label>
                    <Form.Control type="text" value={name} onChange={(e) => setName(e.target.value)} required />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Material</Form.Label>
                    <Form.Select value={material} onChange={(e) => setMaterial(parseInt(e.target.value, 10))}>
                        {Object.entries(materialTypes).map(([key, value]) => (
                            <option key={key} value={key}>{value}</option>
                        ))}
                    </Form.Select>
                </Form.Group>
                <Button variant="primary" type="submit">Save</Button>
            </Form>
        </Container>
    );
};

export default DetailForm;
