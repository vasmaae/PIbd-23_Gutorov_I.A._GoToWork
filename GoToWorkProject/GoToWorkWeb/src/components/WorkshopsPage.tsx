import { useState, useEffect } from 'react';
import { Container, Table, Button, Alert, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getWorkshops, deleteWorkshop } from '../services/workshopService';

const WorkshopsPage = () => {
    const [workshops, setWorkshops] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [workshopToDelete, setWorkshopToDelete] = useState(null);

    useEffect(() => {
        fetchWorkshops();
    }, []);

    const fetchWorkshops = () => {
        setIsLoading(true);
        getWorkshops()
            .then(data => {
                setWorkshops(data || []);
            })
            .catch(() => setError('Failed to fetch workshops'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (workshop) => {
        setWorkshopToDelete(workshop);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setWorkshopToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!workshopToDelete) return;
        try {
            await deleteWorkshop(workshopToDelete.id);
            handleCloseDeleteModal();
            fetchWorkshops(); // Refresh the list
        } catch (err) {
            setError('Failed to delete workshop.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Цеха</h1>
            <Link to="/workshops/new" className="btn btn-primary mb-3">Создать цех</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Адрес</th>
                        <th>Производство</th>
                        <th>Работники</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    {workshops.map(workshop => (
                        <tr key={workshop.id}>
                            <td>{workshop.address}</td>
                            <td>{workshop.productionName || 'N/A'}</td>
                            <td>
                                <ul>
                                    {(workshop.employees || []).map(e => (
                                        <li key={e.employeeId}>{e.employeeName}</li>
                                    ))}
                                </ul>
                            </td>
                            <td>
                                <Link to={`/workshops/edit/${workshop.id}`} className="btn btn-sm btn-warning me-2">Редактировать</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteClick(workshop)}>Удалить</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить цех по адресу "{workshopToDelete?.address}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default WorkshopsPage;
