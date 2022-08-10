import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BIEmployeeResponseDto } from '../../DTOs/bi-employee-response-dto';
import { AuthenticationService } from '../authentication.service';
import { DepartmentsService } from '../department/departments.service';
import { ResponseDto } from '../department/response-dto';

@Injectable({
  providedIn: 'root'
})
export class EmployeesService {

  _BIEmployeeList = new BehaviorSubject<BIEmployeeResponseDto[]>([]);
  BIEmployeeList$ = this._BIEmployeeList.asObservable();

  constructor(private http:HttpClient,private auth:AuthenticationService,private master:DepartmentsService) { }

  uploadEmployees(file:File[]){
    const formData = new FormData();
    formData.append("file",file[0],file[0].name);

    this.http.post("https://localhost:7177/api/Employee/Upload",formData).subscribe(
      (response)=>{
        debugger
        let result = response as ResponseDto;
        this._BIEmployeeList.next(result._Data as BIEmployeeResponseDto[]);
        this.master.getDepartments();
        this.master.getDesignations();
      }
    )
  }

  saveEmployees(employees:BIEmployeeResponseDto[])
  {
    this.http.post("https://localhost:7177/api/Employee/BulkAdd",{"employees":employees},{headers:this.auth.getHeader()})
    .subscribe(
      (response)=>{
        console.log(response);
      }
    )
  }
}
