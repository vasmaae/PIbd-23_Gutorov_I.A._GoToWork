import api from './api';

export const getMachines = async () => {
    const response = await api.get('/Machines/GetAllRecords');
    return response.data;
};

export const getMachine = async (id) => {
    const response = await api.get(`/Machines/GetRecord/${id}`);
    return response.data;
};

export const createMachine = async (machineData) => {
    return await api.post('/Machines/Create', machineData);
};

export const updateMachine = async (machineData) => {
    return await api.put('/Machines/Update', machineData);
};

export const deleteMachine = async (id) => {
    return await api.delete(`/Machines/Delete/${id}`);
};
