import { useState, useEffect } from 'react';
import { Container, Table, Button, Alert, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getProductions, deleteProduction } from '../services/productionService';

const ProductionsPage = () => {
    const [productions, setProductions] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [productionToDelete, setProductionToDelete] = useState(null);

    useEffect(() => {
        fetchProductions();
    }, []);

    const fetchProductions = () => {
        setIsLoading(true);
        getProductions()
            .then(data => {
                setProductions(data || []);
            })
            .catch(() => setError('Failed to fetch productions'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (production) => {
        setProductionToDelete(production);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setProductionToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!productionToDelete) return;
        try {
            await deleteProduction(productionToDelete.id);
            handleCloseDeleteModal();
            fetchProductions(); // Refresh the list
        } catch (err) {
            setError('Failed to delete production.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Производства</h1>
            <Link to="/productions/new" className="btn btn-primary mb-3">Создать производство</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Название</th>
                        <th>Детали</th>
                        <th>Цеха</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    {productions.map(production => (
                        <tr key={production.id}>
                            <td>{production.name}</td>
                            <td>
                                <ul>
                                    {(production.details || []).map(d => (
                                        <li key={d.detailId}>{d.detailName}</li>
                                    ))}
                                </ul>
                            </td>
                            <td>
                                <ul>
                                    {(production.workshops || []).map(w => (
                                        <li key={w.id}>{w.address}</li>
                                    ))}
                                </ul>
                            </td>
                            <td>
                                <Link to={`/productions/edit/${production.id}`} className="btn btn-sm btn-warning me-2">Редактировать</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteClick(production)}>Удалить</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить производство "{productionToDelete?.name}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default ProductionsPage;
