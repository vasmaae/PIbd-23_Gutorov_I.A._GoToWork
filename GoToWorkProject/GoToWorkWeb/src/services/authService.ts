import api from './api';

export const login = async (login, password) => {
    const response = await api.post('/Auth/Login', { login, password });
    if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        try {
            await fetchUser(login);
        } catch (error) {
            console.error("Failed to fetch user data after login:", error);
            // Don't re-throw, allow login to succeed even if user data fetch fails.
        }
    }
    return response.data;
};

export const fetchUser = async (login) => {
    const response = await api.get(`/Users/${login}`);
    if (response.data) {
        localStorage.setItem('user', JSON.stringify(response.data));
    }
};

export const register = async (login, email, password, role) => {
    return await api.post('/Auth/Register', { login, email, password, role });
};

export const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
};

export const getToken = () => {
    return localStorage.getItem('token');
};

export const getUser = () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
};

export const getUserRole = () => {
    const user = getUser();
    // Assuming the role is a number in the user object, like in the UserRole enum
    return user ? user.role : null;
};
