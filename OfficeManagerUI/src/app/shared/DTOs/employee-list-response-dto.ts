export class EmployeeListResponseDto implements IEmployeeListResponseDto {
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

export class EmployeeDto implements IEmployeeDto {
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


export interface IEmployeeSkill{
  employeeId: number;
  skillName: string;
  skillId: number;
  levelId: number;
  rateId: number;
  checked: boolean;
}

export class EmployeeSkill implements IEmployeeSkill{
  employeeId: number;
  skillName: string;
  skillId: number;
  levelId: number;
  rateId: number;
  checked: boolean;

  constructor(){
    this.employeeId = 0;
    this.levelId = 0;
    this.skillId = 0;
    this.rateId = 0;
    this.skillName = '';
    this.checked = false;
  }
}

export interface IEmployeeDetailDto {
  userId: number;
  roleId: number;
  employeeNo: number;
  employeeId: number;
  employeeName: string;
  email: string;
  designationId: number;
  departmentId: number;
  dateOfBirth: Date;
  dateOfJoining: Date;
  skills: IEmployeeSkill[];
}

export class EmployeeDetailDto implements IEmployeeDetailDto {
  userId: number;
  roleId: number;
  employeeNo: number;
  employeeId: number;
  employeeName: string;
  email: string;
  designationId: number;
  departmentId: number;
  dateOfBirth: Date;
  dateOfJoining: Date;
  skills: IEmployeeSkill[];
  constructor(){
    this.userId = 0;
    this.roleId = 0;
    this.email = "";
    this.employeeNo = 0;
    this.employeeId = 0
    this.employeeName = "";
    this.departmentId = 0;
    this.designationId = 0;
    this.dateOfBirth = new Date();
    this.dateOfJoining = new Date();
    this.skills = [];
  }
}
