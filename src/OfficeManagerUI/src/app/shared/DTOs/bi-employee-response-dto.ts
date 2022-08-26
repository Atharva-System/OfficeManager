export class BIEmployeeResponseDto {
  id: number;
  employeeNo: number;
  employeeName: string;
  departmentId: number;
  designationId: number;
  department: string;
  designation: string;
  dateOfJoining: Date;
  dateOfBirth: Date;
  isValid: boolean;
  roleId: number;
  validationErrors: string[];
  constructor()
  {
    this.id = 0;
    this.employeeNo = 0;
    this.employeeName = "";
    this.departmentId = 0;
    this.department = "";
    this.designation = "";
    this.designationId = 0;
    this.dateOfJoining = new Date();
    this.dateOfBirth = new Date();
    this.isValid = true;
    this.roleId = 1;
    this.validationErrors = [];
  }
}
