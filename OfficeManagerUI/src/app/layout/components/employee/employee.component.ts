import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BIEmployeeResponseDto } from 'src/app/shared/DTOs/bi-employee-response-dto';
import { DepartmentResponseDto } from 'src/app/shared/DTOs/department-response-dto';
import { DesignationResponseDto } from 'src/app/shared/DTOs/designation-response-dto';
import { DepartmentsService } from 'src/app/shared/services/department/departments.service';
import { EmployeesService } from 'src/app/shared/services/employee/employees.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {

  uploadForm: FormGroup = new FormGroup({});
  loading: boolean = false;
  file:File = new File([],"");
  employees: BIEmployeeResponseDto[] = [];
  departments: DepartmentResponseDto[] = [];
  designations: DesignationResponseDto[] =[];

  constructor(private builder:FormBuilder,private service:EmployeesService,private master: DepartmentsService) { }

  ngOnInit(): void {
    this.uploadForm = this.builder.group({
      "EployeeSheet":[File,[Validators.required]]
    })
  }

  uploadFile(event:any): void{
    // let path = "";
    // path = this.file;
    const file2:File[] = event.target.files;
    this.service.uploadEmployees(file2);
    this.setPreview();
  }

  setPreview(): void {

    this.service.BIEmployeeList$.subscribe(
      (employee:BIEmployeeResponseDto[])=>{
        this.employees = employee;
      })
    this.master.DepartmentsList$.subscribe(
      (departments:DepartmentResponseDto[])=>{
        this.departments = departments;
      }
    )
    this.master.DesignationsList$.subscribe(
      (designations:DesignationResponseDto[])=>{
        debugger
        this.designations = designations;
      }
    )
  }

  saveAll(): void{
    this.loading = true;
    this.service.saveEmployees(this.employees);
    this.loading = false;
  }

}
