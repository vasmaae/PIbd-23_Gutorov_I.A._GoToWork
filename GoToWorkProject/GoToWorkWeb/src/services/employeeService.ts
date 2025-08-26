import api from './api';

export const getEmployees = async () => {
    const response = await api.get('/Employees/GetAllRecords');
    return response.data;
};

export const getEmployee = async (id) => {
    const response = await api.get(`/Employees/GetRecord/${id}`);
    return response.data;
};

export const createEmployee = async (employeeData) => {
    return await api.post('/Employees/Create', employeeData);
};

export const updateEmployee = async (employeeData) => {
    return await api.put('/Employees/Update', employeeData);
};

export const deleteEmployee = async (id) => {
    return await api.delete(`/Employees/Delete/${id}`);
};