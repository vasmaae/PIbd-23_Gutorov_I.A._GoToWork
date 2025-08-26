import api from './api';

export const getProducts = async () => {
    const response = await api.get('/Products/GetAllRecords');
    return response.data;
};

export const getProduct = async (id: string) => {
    const response = await api.get(`/Products/GetRecord/${id}`);
    return response.data;
};

export const createProduct = async (productData) => {
    return await api.post('/Products/Create', productData);
};

export const updateProduct = async (productData) => {
    return await api.put('/Products/Update', productData);
};

export const deleteProduct = async (id) => {
    return await api.delete(`/Products/Delete/${id}`);
};
