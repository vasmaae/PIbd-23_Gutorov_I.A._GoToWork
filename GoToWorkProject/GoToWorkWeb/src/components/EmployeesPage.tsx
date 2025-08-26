import { useState, useEffect } from 'react';
import { Container, Table, Button, Alert, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { getEmployees, deleteEmployee } from '../services/employeeService';

const EmployeesPage = () => {
    const [employees, setEmployees] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [employeeToDelete, setEmployeeToDelete] = useState(null);

    useEffect(() => {
        fetchEmployees();
    }, []);

    const fetchEmployees = () => {
        setIsLoading(true);
        getEmployees()
            .then(data => {
                setEmployees(data || []);
            })
            .catch(() => setError('Failed to fetch employees'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (employee) => {
        setEmployeeToDelete(employee);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setEmployeeToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!employeeToDelete) return;
        try {
            await deleteEmployee(employeeToDelete.id);
            handleCloseDeleteModal();
            fetchEmployees(); // Refresh the list
        } catch (err) {
            setError('Failed to delete employee.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Работники</h1>
            <Link to="/employees/new" className="btn btn-primary mb-3">Создать работника</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>ФИО</th>
                        <th>Действия</th>
                    </tr>
                </thead>
                <tbody>
                    {employees.map(employee => (
                        <tr key={employee.id}>
                            <td>{employee.fullName}</td>
                            <td>
                                <Link to={`/employees/edit/${employee.id}`} className="btn btn-sm btn-warning me-2">Редактировать</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteClick(employee)}>Удалить</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить работника "{employeeToDelete?.fullName}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default EmployeesPage;
