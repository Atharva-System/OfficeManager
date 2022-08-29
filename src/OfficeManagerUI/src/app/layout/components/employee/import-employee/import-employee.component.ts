import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BIEmployeeResponseDto } from 'src/app/shared/DTOs/bi-employee-response-dto';
import { DepartmentResponseDto } from 'src/app/shared/DTOs/department-response-dto';
import { DesignationResponseDto } from 'src/app/shared/DTOs/designation-response-dto';
import { EmployeeDetailDto } from 'src/app/shared/DTOs/employee-list-response-dto';
import { EmployeesService } from 'src/app/shared/services/employee/employees.service';

@Component({
  selector: 'app-import-employee',
  templateUrl: './import-employee.component.html',
  styleUrls: ['./import-employee.component.scss']
})
export class ImportEmployeeComponent implements OnInit {

  @Input() employees:BIEmployeeResponseDto[] = [];
  @Input() departments: DepartmentResponseDto[] = [];
  @Input() designations: DesignationResponseDto[] =[];
  @Input() Loading$:Observable<boolean> = new Observable<boolean>();

  @Output() save:EventEmitter<boolean> = new EventEmitter<boolean>(false);
  @Output() cancel:EventEmitter<boolean> = new EventEmitter<boolean>(false);

  @Input() invalidMessageVisible:boolean = false;


  constructor(private service:EmployeesService) { }

  ngOnInit(): void {
  }

  saveAll(): void{
    this.save.emit(false);
  }

  cancelUpload() : void{
    this.cancel.emit(false);
  }

  // setDesignationId(isValid:boolean,employeeId:number,designationId:any): void{
  //   if(!isValid)
  //   {
  //     this.employees.filter((emp:BIEmployeeResponseDto)=>emp.id == employeeId)[0].designationId = Number(designationId.target.value);
  //   }
  // }

  // setDepartment(isValid:boolean,employeeId:number,departmentId:any): void {
  //   if(!isValid){
  //     this.employees.filter((emp:BIEmployeeResponseDto)=>emp.id == employeeId)[0].departmentId = Number(departmentId.target.value);
  //   }
  // }

}
