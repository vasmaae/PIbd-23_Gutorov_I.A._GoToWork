import api from './api';

export const getMachines = async () => {
    const response = await api.get('/Machines/GetAllRecords');
    return response.data;
};
