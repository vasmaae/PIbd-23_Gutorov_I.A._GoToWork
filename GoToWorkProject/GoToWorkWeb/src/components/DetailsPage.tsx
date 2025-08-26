import { useState, useEffect } from 'react';
import { Container, Table, Button, Alert, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getDetails, deleteDetail } from '../services/detailService';

const materialTypes = {
    1: 'Steel',
    2: 'Aluminum',
    3: 'Plastic',
    4: 'Copper',
    5: 'Composite'
};

const DetailsPage = () => {
    const [details, setDetails] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [detailToDelete, setDetailToDelete] = useState(null);

    useEffect(() => {
        fetchDetails();
    }, []);

    const fetchDetails = () => {
        setIsLoading(true);
        getDetails()
            .then(data => {
                setDetails(data || []);
            })
            .catch(() => setError('Failed to fetch details'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (detail) => {
        setDetailToDelete(detail);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setDetailToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!detailToDelete) return;
        try {
            await deleteDetail(detailToDelete.id);
            handleCloseDeleteModal();
            fetchDetails(); // Refresh the list
        } catch (err) {
            setError('Failed to delete detail.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Детали</h1>
            <Link to="/details/new" className="btn btn-primary mb-3">Создать деталь</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Название</th>
                        <th>Материал</th>
                        <th>Дата создания</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    {details.map(detail => (
                        <tr key={detail.id}>
                            <td>{detail.name}</td>
                            <td>{materialTypes[detail.material] || 'Unknown'}</td>
                            <td>{new Date(detail.creationDate).toLocaleDateString()}</td>
                            <td>
                                <Link to={`/details/edit/${detail.id}`} className="btn btn-sm btn-warning me-2">Редактировать</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteClick(detail)}>Удалить</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить деталь "{detailToDelete?.name}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default DetailsPage;
