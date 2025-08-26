import { BrowserRouter, Route, Routes } from 'react-router-dom';
import LoginPage from '../components/LoginPage';
import RegisterPage from '../components/RegisterPage';
import MainLayout from '../layouts/MainLayout';

const AppRouter = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<h1>Home Page</h1>} />
                    <Route path="login" element={<LoginPage />} />
                    <Route path="register" element={<RegisterPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
};

export default AppRouter;
