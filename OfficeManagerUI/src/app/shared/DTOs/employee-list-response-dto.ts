export class EmployeeListResponseDto {
  employees: EmployeeDto[];
  totalCount: number;
  pageSize: number;
  totalPages: number;
  pageNumber: number;
  constructor(
    employees: EmployeeDto[],
    totalCount: number,
    pageSize: number,
    totalPages: number,
    pageNumber: number
  )
  {
    this.employees = employees;
    this.totalCount = totalCount;
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.totalPages = totalPages;
  }
}

export interface IEmployeeListResponseDto {
  employees: EmployeeDto[];
  totalCount: number;
  pageSize: number;
  totalPages: number;
  pageNumber: number;
}

export interface IEmployeeDto {
  employeeId: number;
  employeeNo: number;
  employeeName: string;
  emlployeeRole: string;
  department: string;
  designation: string;
  email: string;
  dateOfJoining: Date;
  dateOfBirth: Date;
}

export class EmployeeDto {
  employeeId: number;
  employeeNo: number;
  employeeName: string;
  emlployeeRole: string;
  department: string;
  designation: string;
  email: string;
  dateOfJoining: Date;
  dateOfBirth: Date;

  constructor(
    employeeId:number,
    employeeNo: number,
    employeeName: string,
    employeeRole: string,
    department: string,
    designation: string,
    email: string,
    dateOfJoining: Date,
    dateOfBirth: Date
  )
  {
    this.employeeId = employeeId;
    this.employeeNo = employeeNo;
    this.employeeName = employeeName;
    this.emlployeeRole = employeeRole;
    this.department = department;
    this.designation = designation;
    this.email = email;
    this.dateOfJoining = dateOfJoining;
    this.dateOfBirth = dateOfBirth;
  }
}
