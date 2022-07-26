import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthenticationservicesService } from '../Authentication/authenticationservices.service';
import { BehaviorSubject } from 'rxjs';
import { DesignationDto } from '../Authentication/dtos/DesignationDto';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class DesignationService {

  _designationList = new BehaviorSubject<DesignationDto[]>([]);
  designationList$ = this._designationList.asObservable();

  constructor(private http:HttpClient,private authService:AuthenticationservicesService,private toastr:ToastrService) { }

  getDesignations(search:string): void{
    this.http.get(environment.baseRoute + "Designation?search="+search,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this._designationList.next(result as DesignationDto[]);
    });
  }

  addDesignation(data:DesignationDto): void {
    this.http.post(environment.baseRoute + "Designation/Add/",data,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this.toastr.success("Designation added successfully!");
      this.getDesignations('');
    },
    (error)=>{
      console.log(error);
      this.toastr.error("Something went wrong!");
    });
  }

  deleteDesignation(id:string){
    this.http.delete(environment.baseRoute + `Designation/${id}/Delete`,{headers:this.authService.getHeader()})
    .subscribe((result)=>{
      this.toastr.success("Designation deleted successfully!");
      this.getDesignations('');
    },(error)=>{
      console.log(error);
      this.toastr.error("Something went wrong!");
    });
  }
}
