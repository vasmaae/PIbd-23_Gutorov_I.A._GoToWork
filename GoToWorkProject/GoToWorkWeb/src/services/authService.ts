import api from './api';

export const login = async (login, password) => {
    const response = await api.post('/Auth/Login', { login, password });
    if (response.data.token) {
        localStorage.setItem('token', response.data.token);
    }
    return response.data;
};

export const register = async (login, email, password, role) => {
    return await api.post('/Auth/Register', { login, email, password, role });
};

export const logout = () => {
    localStorage.removeItem('token');
};

export const getToken = () => {
    return localStorage.getItem('token');
};
