import React, { useState, useEffect } from 'react';
import { Form, Button, Container, Row, Col, Alert } from 'react-bootstrap';
import { getWorkshopsReportXlsx, getWorkshopsReportDocx, getDetailsReportPdf, getDetailsReportPdfEmail, getDetailsList } from '../services/reportService';

interface DetailViewModel {
    id: number;
    name: string;
}

const ReportsPage: React.FC = () => {
    const [detailsList, setDetailsList] = useState<DetailViewModel[]>([]);
    const [selectedWorkshopsDetailIds, setSelectedWorkshopsDetailIds] = useState<number[]>([]);
    const [detailsStartDate, setDetailsStartDate] = useState<string>('');
    const [detailsEndDate, setDetailsEndDate] = useState<string>('');
    const [detailsEmail, setDetailsEmail] = useState<string>('');
    const [message, setMessage] = useState<{ type: 'success' | 'danger'; text: string } | null>(null);

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const data = await getDetailsList();
                setDetailsList(data);
            } catch (error: any) {
                setMessage({ type: 'danger', text: `Ошибка при загрузке деталей: ${error.message}` });
            }
        };
        fetchDetails();
    }, []);

    const handleDownload = async (serviceCall: Function, data: any, filename: string, contentType: string) => {
        try {
            const fileBlob = await serviceCall(data);
            const url = window.URL.createObjectURL(new Blob([fileBlob], { type: contentType }));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.remove();
            setMessage({ type: 'success', text: `${filename} успешно загружен!` });
        } catch (error: any) {
            const errorText = error.response ? await error.response.data.text() : error.message;
            setMessage({ type: 'danger', text: `Ошибка при загрузке ${filename}: ${errorText}` });
        }
    };

    const handleEmail = async (serviceCall: Function, data: any, successMessage: string) => {
        try {
            await serviceCall(data);
            setMessage({ type: 'success', text: successMessage });
        } catch (error: any) {
            const errorText = error.response ? await error.response.data.text() : error.message;
            setMessage({ type: 'danger', text: `Ошибка при отправке письма: ${errorText}` });
        }
    };

    const handleDetailCheckboxChange = (detailId: number) => {
        setSelectedWorkshopsDetailIds(prevSelected =>
            prevSelected.includes(detailId)
                ? prevSelected.filter(id => id !== detailId)
                : [...prevSelected, detailId]
        );
    };

    const getWorkshopsReportXlsxHandler = async () => {
        await handleDownload(getWorkshopsReportXlsx, { detailIds: selectedWorkshopsDetailIds }, 'WorkshopsReport.xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    };

    const getWorkshopsReportDocxHandler = async () => {
        await handleDownload(getWorkshopsReportDocx, { detailIds: selectedWorkshopsDetailIds }, 'WorkshopsReport.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document');
    };

    const getDetailsReportPdfHandler = async () => {
        await handleDownload(getDetailsReportPdf, {
            startDate: detailsStartDate ? new Date(detailsStartDate).toISOString() : null,
            endDate: detailsEndDate ? new Date(detailsEndDate).toISOString() : null
        }, 'DetailsReport.pdf', 'application/pdf');
    };

    const getDetailsReportPdfEmailHandler = async () => {
        await handleEmail(getDetailsReportPdfEmail, {
            startDate: detailsStartDate ? new Date(detailsStartDate).toISOString() : null,
            endDate: detailsEndDate ? new Date(detailsEndDate).toISOString() : null,
            email: detailsEmail
        }, 'Отчет по деталям PDF успешно отправлен на почту!');
    };

    return (
        <Container className="mt-4">
            <h2 className="mb-4">Отчеты</h2>

            {message && <Alert variant={message.type}>{message.text}</Alert>}

            <Row className="mb-4">
                <Col>
                    <h4>Отчет по цехам</h4>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Выберите детали</Form.Label>
                            {detailsList.length > 0 ? (
                                detailsList.map(detail => (
                                    <Form.Check
                                        key={detail.id}
                                        type="checkbox"
                                        id={`detail-${detail.id}`}
                                        label={detail.name}
                                        checked={selectedWorkshopsDetailIds.includes(detail.id)}
                                        onChange={() => handleDetailCheckboxChange(detail.id)}
                                    />
                                ))
                            ) : (
                                <p>Загрузка деталей...</p>
                            )}
                        </Form.Group>
                        <Button variant="primary" onClick={getWorkshopsReportXlsxHandler} className="me-2">
                            Скачать XLSX
                        </Button>
                        <Button variant="primary" onClick={getWorkshopsReportDocxHandler}>
                            Скачать DOCX
                        </Button>
                    </Form>
                </Col>
            </Row>

            <Row className="mb-4">
                <Col>
                    <h4>Отчет по деталям</h4>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Дата начала</Form.Label>
                            <Form.Control
                                type="date"
                                value={detailsStartDate}
                                onChange={(e) => setDetailsStartDate(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Дата окончания</Form.Label>
                            <Form.Control
                                type="date"
                                value={detailsEndDate}
                                onChange={(e) => setDetailsEndDate(e.target.value)}
                            />
                        </Form.Group>
                        <Button variant="primary" onClick={getDetailsReportPdfHandler} className="me-2">
                            Скачать PDF
                        </Button>
                        <Form.Group className="mb-3 mt-3">
                            <Form.Label>Email для отправки PDF</Form.Label>
                            <Form.Control
                                type="email"
                                value={detailsEmail}
                                onChange={(e) => setDetailsEmail(e.target.value)}
                                placeholder="Введите адрес электронной почты"
                            />
                        </Form.Group>
                        <Button variant="primary" onClick={getDetailsReportPdfEmailHandler}>
                            Отправить PDF по Email
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};

export default ReportsPage;
