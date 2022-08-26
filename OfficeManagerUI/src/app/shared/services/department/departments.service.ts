import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DepartmentResponseDto } from '../../DTOs/department-response-dto';
import { DesignationResponseDto } from '../../DTOs/designation-response-dto';
import { AuthenticationService } from '../authentication.service';
import { ResponseDto } from './response-dto';

@Injectable({
  providedIn: 'root'
})
export class DepartmentsService {
  _DepartmentsList = new BehaviorSubject<DepartmentResponseDto[]>([]);
  DepartmentsList$ = this._DepartmentsList.asObservable();

  _DesignationsList = new BehaviorSubject<DepartmentResponseDto[]>([]);
  DesignationsList$ = this._DesignationsList.asObservable();

  constructor(private http:HttpClient,private auth:AuthenticationService) { }

  getDepartments(){
    this.http.get("https://localhost:7177/api/Masters/Departments",{headers:this.auth.getHeader()}).subscribe(
      (response)=>{
        let result = response as ResponseDto;
        this._DepartmentsList.next(result._Data as DepartmentResponseDto[]);
      },
      (err)=>{
        var error = err.error as ResponseDto;
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
      }
    )
  }

  getDesignations(){
    this.http.get("https://localhost:7177/api/Masters/Designations",{headers:this.auth.getHeader()}).subscribe(
      (response)=>{
        let result = response as ResponseDto;
        this._DesignationsList.next(result._Data as DesignationResponseDto[]);
      },
      (err)=>{
        var error = err.error as ResponseDto;
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
      }
    )
  }
}
