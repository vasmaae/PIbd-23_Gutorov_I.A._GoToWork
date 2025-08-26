import { useState, useEffect } from 'react';
import { Container, Table, Button, Alert, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getMachines, deleteMachine } from '../services/machineService';
import { MachineType } from '../types/types';

const MachinesPage = () => {
    const [machines, setMachines] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [machineToDelete, setMachineToDelete] = useState(null);

    useEffect(() => {
        fetchMachines();
    }, []);

    const fetchMachines = () => {
        setIsLoading(true);
        getMachines()
            .then(data => {
                setMachines(data || []);
            })
            .catch(() => setError('Failed to fetch machines'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (machine) => {
        setMachineToDelete(machine);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setMachineToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!machineToDelete) return;
        try {
            await deleteMachine(machineToDelete.id);
            handleCloseDeleteModal();
            fetchMachines(); // Refresh the list
        } catch (err) {
            setError('Failed to delete machine.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Станки</h1>
            <Link to="/machines/new" className="btn btn-primary mb-3">Создать станок</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Модель</th>
                        <th>Тип</th>
                        <th>Работники</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    {machines.map(machine => (
                        <tr key={machine.id}>
                            <td>{machine.model}</td>
                            <td>{MachineType[machine.type]}</td>
                            <td>
                                <ul>
                                    {(machine.employees || []).map(e => (
                                        <li key={e.employeeId}>{e.employeeName}</li>
                                    ))}
                                </ul>
                            </td>
                            <td>
                                <Link to={`/machines/edit/${machine.id}`} className="btn btn-sm btn-warning me-2">Редактировать</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteClick(machine)}>Удалить</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить станок "{machineToDelete?.model}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default MachinesPage;
