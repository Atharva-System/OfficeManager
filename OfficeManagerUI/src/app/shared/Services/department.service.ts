import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthenticationservicesService } from '../Authentication/authenticationservices.service';
import { BehaviorSubject } from 'rxjs';
import { DepartmentDto } from '../Authentication/dtos/DepartmentDto';
import { ToastrService } from 'ngx-toastr';
import { ResultDto } from '../Authentication/dtos/ResultDto';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private http:HttpClient,private authService:AuthenticationservicesService,private toastr:ToastrService) { }

  _departmentList = new BehaviorSubject<DepartmentDto[]>([]);
  departmentList$ = this._departmentList.asObservable();

  getDepartments(search:string): void{
    this.http.get(environment.baseRoute + "Department?search="+search,{headers:this.authService.getHeader()}).subscribe(
      (result)=>{
        this._departmentList.next(result as DepartmentDto[]);
      }
    )
  }

  addDepartment(data:DepartmentDto): void{
    this.http.post(environment.baseRoute + "Department/Add",data,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this.toastr.success("Department added successfully!");
      this.getDepartments('');
    },(error)=>{
      let response = error.error as ResultDto;
      if(response.errors && response.errors.length > 0)
      {
        for(let error in response.errors)
        {
          this.toastr.error(response.errors[error]);
        }
      }
      else{
        this.toastr.error(response.message);
      }
    });
  }

  editDepartment(data:DepartmentDto): void{
    this.http.put(environment.baseRoute + "Department/Edit",data,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this.toastr.success("Department updated successfully!");
      this.getDepartments('');
    },(error)=>{
      let response = error.error as ResultDto;
      if(response.errors && response.errors.length > 0)
      {
        for(let error in response.errors)
        {
          this.toastr.error(response.errors[error]);
        }
      }
      else{
        this.toastr.error(response.message);
      }
    });
  }

  deleteDepartment(id:string):void {
    this.http.delete(environment.baseRoute + `Department/${id}/Delete`,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this.toastr.success("Department deleted successfully!");
      this.getDepartments('');
    },(error)=>{
      let response = error.error as ResultDto;
      if(response.errors && response.errors.length > 0)
      {
        for(let error in response.errors)
        {
          this.toastr.error(response.errors[error]);
        }
      }
      else{
        this.toastr.error(response.message);
      }
    });
  }

}
