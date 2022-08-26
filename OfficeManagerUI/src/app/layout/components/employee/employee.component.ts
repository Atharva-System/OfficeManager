import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { BIEmployeeResponseDto } from 'src/app/shared/DTOs/bi-employee-response-dto';
import { DepartmentResponseDto } from 'src/app/shared/DTOs/department-response-dto';
import { DesignationResponseDto } from 'src/app/shared/DTOs/designation-response-dto';
import { EmployeeDto, EmployeeListResponseDto, IEmployeeListResponseDto } from 'src/app/shared/DTOs/employee-list-response-dto';
import { DepartmentsService } from 'src/app/shared/services/department/departments.service';
import { EmployeesService } from 'src/app/shared/services/employee/employees.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {

  uploadForm: FormGroup = new FormGroup({});
  employees: BIEmployeeResponseDto[] = [];
  departments: DepartmentResponseDto[] = [];
  designations: DesignationResponseDto[] =[];
  upload:boolean = false;

  Loading$:Observable<boolean> = new Observable<boolean>();
  EmployeeListResponse$:Observable<EmployeeListResponseDto> = new Observable<EmployeeListResponseDto>();
  EmployeeList$:Observable<EmployeeDto[]> = new Observable<EmployeeDto[]>();

  @ViewChild('fileUpload') fileInput:any;

  //Search Properties
  pageNo = 1;
  pageSize = 10;
  search = "";
  department = 0;
  designation = 0;
  fromDate = "";
  toDate = "";

  constructor(private builder:FormBuilder,private service:EmployeesService,private master: DepartmentsService) {
    this.Loading$ = this.service.Loading$;
    this.EmployeeList$ = this.service.EmployeeList$;
    this.EmployeeListResponse$ = this.service.EmployeeListResponse$;
    this.pageNo = 1;
  }




  ngOnInit(): void {
    this.uploadForm = this.builder.group({
      "EployeeSheet":[File,[Validators.required]]
    })
    this.master.getDepartments();
    this.master._DepartmentsList.subscribe(
      (departments:DepartmentResponseDto[]) => {
        this.departments = departments;
      }
    )
    this.master.getDesignations();
    this.master._DesignationsList.subscribe(
      (designations:DesignationResponseDto[]) => {
        this.designations = designations;
      }
    )
    this.searchEmployee();
  }

  openBrowse(): void{
    document.getElementsByName('file')[0].click();
  }

  uploadFile(event:any): void{
    // let path = "";
    // path = this.file;
    this.upload = true;
    const file2:File[] = event.target.files;
    this.service.uploadEmployees(file2);
    this.setPreview();
    this.fileInput.nativeElement.value = '';
  }

  searchEmployee(): void{
    this.service.getAllEmployees(this.search,this.department,this.designation,0,this.fromDate,this.toDate,this.fromDate,this.toDate,this.pageNo,this.pageSize);

  }

  createRange(number:number){
    // return new Array(number);
    return new Array(number).fill(0)
      .map((n, index) => index + 1);
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
        this.designations = designations;
      }
    )
  }

  paginate(pageNo:number): void {
    this.pageNo = pageNo;
    this.searchEmployee();
  }

  saveAll(): void{
    this.service.saveEmployees(this.employees);
    this.upload = false;
  }

  cancelUpload():void {
    this.employees = [];
    this.upload = false;
    this.fileInput.nativeElement.value = '';
  }

}
