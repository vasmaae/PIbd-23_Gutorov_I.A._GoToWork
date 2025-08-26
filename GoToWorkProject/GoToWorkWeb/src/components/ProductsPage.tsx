import {useState, useEffect} from 'react';
import {Container, Table, Button, Alert, Modal} from 'react-bootstrap';
import {Link} from 'react-router-dom';
import {getProducts, deleteProduct} from '../services/productService';

const ProductsPage = () => {
    const [products, setProducts] = useState([]);
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [productToDelete, setProductToDelete] = useState(null);

    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = () => {
        setIsLoading(true);
        getProducts()
            .then(data => {
                setProducts(data || []);
            })
            .catch(() => setError('Failed to fetch products'))
            .finally(() => setIsLoading(false));
    };

    const handleDeleteClick = (product) => {
        setProductToDelete(product);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        setShowDeleteModal(false);
        setProductToDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!productToDelete) return;
        try {
            await deleteProduct(productToDelete.id);
            handleCloseDeleteModal();
            fetchProducts(); // Refresh the list
        } catch (err) {
            setError('Failed to delete product.');
            handleCloseDeleteModal();
        }
    };

    if (isLoading) return <p>Loading...</p>;

    return (
        <Container>
            <h1>Изделия</h1>
            <Link to="/products/new" className="btn btn-primary mb-3">Создать изделие</Link>
            {error && <Alert variant="danger">{error}</Alert>}
            <Table striped bordered hover responsive>
                <thead>
                <tr>
                    <th>Название</th>
                    <th>Станок</th>
                    <th>Детали</th>
                    <th>Дата создания</th>
                    <th>Действия</th>
                </tr>
                </thead>
                <tbody>
                {products.map(product => (
                    <tr key={product.id}>
                        <td>{product.name}</td>
                        <td>{product.machineName || 'N/A'}</td>
                        <td>
                            <ul>
                                {(product.details || []).map(d => (
                                    <li key={d.detailId}>{d.detailName}: {d.quantity} шт.</li>
                                ))}
                            </ul>
                        </td>
                        <td>{new Date(product.creationDate).toLocaleDateString()}</td>
                        <td>
                            <Link to={`/products/edit/${product.id}`}
                                  className="btn btn-sm btn-warning me-2">Редактировать</Link>
                            <Button variant="danger" size="sm"
                                    onClick={() => handleDeleteClick(product)}>Удалить</Button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </Table>

            <Modal show={showDeleteModal} onHide={handleCloseDeleteModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить изделие "{productToDelete?.name}"?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseDeleteModal}>Отмена</Button>
                    <Button variant="danger" onClick={handleConfirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default ProductsPage;
