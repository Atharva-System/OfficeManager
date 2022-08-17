import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BIEmployeeResponseDto } from '../../DTOs/bi-employee-response-dto';
import { EmployeeDto, EmployeeListResponseDto, IEmployeeListResponseDto } from '../../DTOs/employee-list-response-dto';
import { AuthenticationService } from '../authentication.service';
import { DepartmentsService } from '../department/departments.service';
import { ResponseDto } from '../department/response-dto';
import { filter, pairwise } from 'rxjs/operators';
import { Router, RoutesRecognized } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class EmployeesService {

  _BIEmployeeList = new BehaviorSubject<BIEmployeeResponseDto[]>([]);
  public BIEmployeeList$ = this._BIEmployeeList.asObservable();

  _EmployeeListResponse = new BehaviorSubject<IEmployeeListResponseDto>({} as EmployeeListResponseDto);
  public EmployeeListResponse$ = this._EmployeeListResponse.asObservable();

  _EmployeeList = new BehaviorSubject<EmployeeDto[]>([]);
  public EmployeeList$ = this._EmployeeList.asObservable();

  _Loading = new BehaviorSubject<boolean>(false);
  public Loading$ = this._Loading.asObservable();

  public get EmployeeList(): EmployeeDto[]{
    return this._EmployeeList.getValue();
  }

  public set EmployeeList(value:EmployeeDto[]){
    this._EmployeeList.next(value);
  }

  //_EmployeeList = new BehaviorSubject<>

  constructor(private http:HttpClient,private auth:AuthenticationService,private master:DepartmentsService,
    private router:Router) { }

  uploadEmployees(file:File[]){
    const formData = new FormData();
    formData.append("file",file[0],file[0].name);

    this.http.post("https://localhost:7177/api/Employee/Upload",formData).subscribe(
      (response)=>{
        let result = response as ResponseDto;
        this._BIEmployeeList.next(result._Data as BIEmployeeResponseDto[]);
        this.master.getDepartments();
        this.master.getDesignations();
      }
    )
  }

  getAllEmployees(search:string,departmentId:number,designationId:number,roleId:number,dobFrom:string,dobTo:string
    ,dojFrom:string,dojTo:string,pageNo:number,pageSize:number): void{
      this._Loading.next(true);
      this.http.get(`https://localhost:7177/api/Employee/GetAll?search=${search}&departmentId=${departmentId}
        &designationId=${designationId}&roleId=${roleId}&dobFrom=${dobFrom}&dobTo=${dobTo}
        &dojFrom=${dojFrom}&dojTo=${dojTo}&pageNo=${pageNo}&pageSize=${pageSize}`)
      .subscribe((response)=>{
        var res = response as ResponseDto;
        var data = res._Data as EmployeeListResponseDto;
        this._EmployeeListResponse.next(data);
        this._EmployeeList.next(data.employees)
        this._Loading.next(false);
      },
      (err)=>{
        console.log(err);
        var error = err.error as ResponseDto;
        this._Loading.next(false);
        if(error._Errors != null)
          alert(error._Errors);
        else
          alert(error._Message);
        this._EmployeeList.next([]);
      })
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

  goBack(){
    this.router.events
    .pipe(filter((evt: any) => evt instanceof RoutesRecognized), pairwise())
    .subscribe((events: RoutesRecognized[]) => {
      this.router.navigateByUrl(events[0].urlAfterRedirects);
    });
  }
}
