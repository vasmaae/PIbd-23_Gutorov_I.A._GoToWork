import api from './api';

interface WorkshopsReportBindingModel {
    detailIds: number[];
}

interface DetailsReportBindingModel {
    startDate: string;
    endDate: string;
    email?: string;
}

export const getWorkshopsReportXlsx = async (data: WorkshopsReportBindingModel) => {
    const response = await api.post('/Reports/GetWorkshopsReportXlsx', data, {
        responseType: 'blob',
    });
    return response.data;
};

export const getWorkshopsReportDocx = async (data: WorkshopsReportBindingModel) => {
    const response = await api.post('/Reports/GetWorkshopsReportDocx', data, {
        responseType: 'blob',
    });
    return response.data;
};

export const getDetailsReportPdf = async (data: DetailsReportBindingModel) => {
    const response = await api.post('/Reports/GetDetailsReportPdf', data, { responseType: 'blob' });
    return response.data;
};

export const getDetailsReportPdfEmail = async (data: DetailsReportBindingModel) => {
    return await api.post('/Reports/GetDetailsReportPdfEmail', data);
};

export const getDetailsList = async () => {
    const response = await api.get('/Details/GetAllRecords');
    return response.data;
};