import api from './api';

export const getDetails = async () => {
    const response = await api.get('/Details/GetAllRecords');
    return response.data;
};

export const getDetail = async (id) => {
    const response = await api.get(`/Details/GetRecord/${id}`);
    return response.data;
};

export const createDetail = async (detailData) => {
    return await api.post('/Details/Register', detailData);
};

export const updateDetail = async (detailData) => {
    return await api.put('/Details/ChangeInfo', detailData);
};

export const deleteDetail = async (id) => {
    return await api.delete(`/Details/Delete/${id}`);
};
