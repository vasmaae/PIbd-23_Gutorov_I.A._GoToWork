import { BrowserRouter, Route, Routes } from 'react-router-dom';
import LoginPage from '../components/LoginPage';
import RegisterPage from '../components/RegisterPage';
import MainLayout from '../layouts/MainLayout';
import DetailsPage from '../components/DetailsPage';
import ProductsPage from '../components/ProductsPage';
import ProductionsPage from '../components/ProductionsPage';
import EmployeesPage from '../components/EmployeesPage';
import MachinesPage from '../components/MachinesPage';
import WorkshopsPage from '../components/WorkshopsPage';
import ReportsPage from '../components/ReportsPage';
import DetailForm from '../components/DetailForm';
import ProductForm from '../components/ProductForm';
import ProductionForm from '../components/ProductionForm';

const AppRouter = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<h1>Главная страница</h1>} />
                    <Route path="login" element={<LoginPage />} />
                    <Route path="register" element={<RegisterPage />} />
                    <Route path="details" element={<DetailsPage />} />
                    <Route path="details/new" element={<DetailForm />} />
                    <Route path="details/edit/:id" element={<DetailForm />} />
                    <Route path="products" element={<ProductsPage />} />
                    <Route path="products/new" element={<ProductForm />} />
                    <Route path="products/edit/:id" element={<ProductForm />} />
                    <Route path="productions" element={<ProductionsPage />} />
                    <Route path="productions/new" element={<ProductionForm />} />
                    <Route path="productions/edit/:id" element={<ProductionForm />} />
                    <Route path="employees" element={<EmployeesPage />} />
                    <Route path="machines" element={<MachinesPage />} />
                    <Route path="workshops" element={<WorkshopsPage />} />
                    <Route path="reports" element={<ReportsPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
};

export default AppRouter;
