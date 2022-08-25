import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BIEmployeeResponseDto } from '../../DTOs/bi-employee-response-dto';
import { EmployeeDetailDto, EmployeeDto, EmployeeListResponseDto, IEmployeeDetailDto, IEmployeeListResponseDto } from '../../DTOs/employee-list-response-dto';
import { AuthenticationService } from '../authentication.service';
import { DepartmentsService } from '../department/departments.service';
import { ResponseDto } from '../department/response-dto';
import { filter, pairwise } from 'rxjs/operators';
import { Router, RoutesRecognized } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeesService {

  private EmployeeId: number = 0;

  public get employeeId(): number{
    return this.EmployeeId;
  }

  public set employeeId(value:number){
    this.EmployeeId = value;
  }

  _BIEmployeeList = new BehaviorSubject<BIEmployeeResponseDto[]>([]);
  public BIEmployeeList$ = this._BIEmployeeList.asObservable();

  _EmployeeListResponse = new BehaviorSubject<IEmployeeListResponseDto>({} as EmployeeListResponseDto);
  public EmployeeListResponse$ = this._EmployeeListResponse.asObservable();

  _EmployeeList = new BehaviorSubject<EmployeeDto[]>([]);
  public EmployeeList$ = this._EmployeeList.asObservable();

  _Loading = new BehaviorSubject<boolean>(false);
  public Loading$ = this._Loading.asObservable();

  _EmployeeDetail = new BehaviorSubject<IEmployeeDetailDto>({} as IEmployeeDetailDto);
  EmployeeDetail$ = this._EmployeeDetail.asObservable();

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

    this.http.post("https://localhost:7177/api/Employee/UploadBulkEmployeeImportData",formData).subscribe(
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
        &dojFrom=${dojFrom}&dojTo=${dojTo}&pageNo=${pageNo}&pageSize=${pageSize}`,{headers:this.auth.getHeader()})
      .subscribe((res)=>{
        var response = res as ResponseDto;
        var data = response._Data as EmployeeListResponseDto;
        this._EmployeeListResponse.next(data);
        this._EmployeeList.next(data.employees)
        this._Loading.next(false);
      },
      (err)=>{
        var error = err.error as ResponseDto;
        this._Loading.next(false);
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
        this._EmployeeList.next([])
      })
  }

  saveEmployees(employees:BIEmployeeResponseDto[])
  {
    this._Loading.next(true);
    this.http.post(environment.baseRoute+"/Employee/SaveBulkEmployees",{"employees":employees},{headers:this.auth.getHeader()})
    .subscribe(
      (response)=>{
        this._Loading.next(false);
        this.router.navigateByUrl('/Employees');
      }
    )
  }

  updateEmployee(employee:EmployeeDetailDto): void{
    this.http.put(environment.baseRoute + '/Employee/Edit',employee,{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        var response = res as ResponseDto;
        alert(response._Message)
        this.router.navigateByUrl('/employees');
      }
    )
  }

  addEmployee(employee:EmployeeDetailDto): void{
    this.http.post(environment.baseRoute + '/Employee/add',employee,{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        var response = res as ResponseDto;
        alert(response._Message)
        this.router.navigateByUrl('/employees');
      }
    )
  }

  getEmployeeDetail(): void {
    this.http.get("https://localhost:7177/api/Employee/Detail/"+this.EmployeeId,{headers:this.auth.getHeader()})
    .subscribe((res)=>{
      let response = res as ResponseDto;
      if(response._StatusCode == '200')
      {
        this._EmployeeDetail.next(response._Data as EmployeeDetailDto);
      }
    })
  }

  goBack(){
    this.router.events
    .pipe(filter((evt: any) => evt instanceof RoutesRecognized), pairwise())
    .subscribe((events: RoutesRecognized[]) => {
      this.router.navigateByUrl(events[0].urlAfterRedirects);
    });
  }
}
