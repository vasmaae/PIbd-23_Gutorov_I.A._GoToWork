export enum MaterialType {
    None = 0,
    Steel = 1,
    Aluminum = 2,
    Plastic = 3,
    Copper = 4,
    Composite = 5
}

export enum UserRole {
    None = 0,
    Executor = 1,
    Guarantor = 2
}

export enum MachineType {
    None = 0,
    Lathe = 1,
    MillingMachine = 2,
    DrillPress = 3,
    BandSaw = 4,
    CNC = 5
}

export interface DetailBindingModel {
    id?: string;
    name?: string;
    material: MaterialType;
    creationDate: string;
}

export interface DetailProductBindingModel {
    detailId?: string;
    productId?: string;
    quantity: number;
}

export interface DetailProductionBindingModel {
    detailId?: string;
    productionId?: string;
}

export interface EmailBindingModel {
    to: string;
    subject: string;
    body: string;
    attachmentPath?: string;
}

export interface EmployeeBindingModel {
    id?: string;
    fullName?: string;
}

export interface EmployeeMachineBindingModel {
    employeeId?: string;
    machineId?: string;
}

export interface EmployeeWorkshopBindingModel {
    employeeId?: string;
    workshopId?: string;
}

export interface MachineBindingModel {
    id?: string;
    model?: string;
    type: MachineType;
    employees?: EmployeeMachineBindingModel[];
}

export interface ProductBindingModel {
    id?: string;
    machineId?: string;
    name?: string;
    creationDate: string;
    details?: DetailProductBindingModel[];
}

export interface ProductionBindingModel {
    id?: string;
    name?: string;
    details?: DetailProductionBindingModel[];
}

export interface ReportBindingModel {
    dateFrom?: string;
    dateTo?: string;
}

export interface UserBindingModel {
    id?: string;
    login?: string;
    email?: string;
    password?: string;
    role: UserRole;
}

export interface UserLoginBindingModel {
    login: string;
    password: string;
}

export interface UserRegisterBindingModel {
    login?: string;
    email?: string;
    password?: string;
    role: UserRole;
}

export interface WorkshopBindingModel {
    id?: string;
    productionId?: string;
    address?: string;
    employees?: EmployeeWorkshopBindingModel[];
}
