import api from './api';

export const getWorkshops = async () => {
    const response = await api.get('/Workshops/GetAllRecords');
    return response.data;
};

export const getWorkshop = async (id) => {
    const response = await api.get(`/Workshops/GetRecord/${id}`);
    return response.data;
};

export const createWorkshop = async (workshopData) => {
    return await api.post('/Workshops/Create', workshopData);
};

export const updateWorkshop = async (workshopData) => {
    return await api.put('/Workshops/Update', workshopData);
};

export const deleteWorkshop = async (id) => {
    return await api.delete(`/Workshops/Delete/${id}`);
};
