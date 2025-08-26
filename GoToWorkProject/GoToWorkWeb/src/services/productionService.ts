import api from './api';

export const getProductions = async () => {
    const response = await api.get('/Productions/GetAllRecords');
    return response.data;
};

export const getProduction = async (id) => {
    const response = await api.get(`/Productions/GetRecord/${id}`);
    return response.data;
};

export const createProduction = async (productionData) => {
    return await api.post('/Productions/Create', productionData);
};

export const updateProduction = async (productionData) => {
    return await api.put('/Productions/Update', productionData);
};

export const deleteProduction = async (id) => {
    return await api.delete(`/Productions/Delete/${id}`);
};
