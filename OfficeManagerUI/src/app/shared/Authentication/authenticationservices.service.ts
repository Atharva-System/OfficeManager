import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterEmployeeDto } from './dtos/RegisterUserDto';
import { ApplicationRolesDto, UserRoleDto } from './dtos/UserRoleDto';
import { LoginDto, loginResponseDto } from './dtos/loginDto';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { WeatherForecastDto } from './dtos/WeatherForecastDto';
import { UserProfileDto } from './dtos/UserProfileDto';
import { DepartmentDto } from './dtos/DepartmentDto';
import { DesignationDto } from './dtos/DesignationDto';
import { ForgotPasswordDto } from './dtos/FrogotPasswordDto';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationservicesService {

  roles:UserRoleDto[] = [];
  _role = new BehaviorSubject<UserRoleDto[]>([]);
  role$ = this._role.asObservable();

  forgotPasswordMailSent$ = new Observable<boolean>();

  _appRoles = new BehaviorSubject<ApplicationRolesDto[]>([]);
  appRoles$ = this._appRoles.asObservable();
  departments$ = new Observable<DepartmentDto[]>();
  designations$ = new Observable<DesignationDto[]>();

  _weatherForecast = new BehaviorSubject<WeatherForecastDto[]>([]);
  weatherForecast$ = this._weatherForecast.asObservable();

  _userProfile = new BehaviorSubject<UserProfileDto>(new UserProfileDto("","","",new UserRoleDto("","",""),"",new Date()));
  userProfile$ = this._userProfile.asObservable();

  loginResponse: loginResponseDto = new loginResponseDto("","","","",[]);

  constructor(private http:HttpClient,private toastr:ToastrService,private router:Router) { }

  getHeader() : HttpHeaders {
    if(localStorage.getItem('token'))
    {
      return new HttpHeaders({
        'Content-Type':'application/json',
        'Authorization':'Bearer ' + localStorage.getItem('token')?? ''
      });
    }
    else{
      return new HttpHeaders({
        'Content-Type':'application/json'
      });
    }
  }

  getApplicationUserRoles(): void{
    this.http.get(environment.baseRoute + "UserRoles/GetAll",{headers:this.getHeader()}).subscribe((result)=>{
      this._appRoles.next(result as ApplicationRolesDto[]);
    });
  }

  getDepartments(search:string): void{
    this.departments$ = this.http.get(environment.baseRoute + "Department?search="+search,{headers:this.getHeader()}).pipe(map((result)=>{
      return result as DepartmentDto[];
    }));
  }

  getDesignations(search:string): void{
    this.designations$ = this.http.get(environment.baseRoute + "Designation?search="+search,{headers:this.getHeader()}).pipe(map((result)=>{
      return result as DesignationDto[];
    }));
  }

  addEmployee(data:RegisterEmployeeDto): void{
    this.http.post(environment.baseRoute + "User/Register",data,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Employee registered successfully.");
      this.getApplicationUserRoles();
    });
  }

  deleteRole(id:string):any{
    return this.http.delete(environment.baseRoute + "UserRoles/delete/"+id,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Role deleted successfully");
      this.getApplicationUserRoles();
      return true;
    },
    (error)=>{
      this.toastr.error("Something went wrong.");
      return false;
    })
  }

  addRole(data:ApplicationRolesDto)
  {
    this.http.post(environment.baseRoute + "UserRoles",data,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Role added successfully");
      this.getApplicationUserRoles();
    })
  }

  loginUser(data:LoginDto)
  {
    this.http.post(environment.baseRoute + "User/Login",data,{headers:this.getHeader()})
    .subscribe((result)=>{
      this.loginResponse = result as loginResponseDto;
      localStorage.setItem("token",this.loginResponse.token);
      this.toastr.success("User logged in successfully.");
      this.router.navigate(['/main']);
    })
  }

  getUserProfile()
  {
    debugger
    this.userProfile$ = this.http.get(environment.baseRoute + "User/"+this.loginResponse.userId,{headers:this.getHeader()}).pipe(map((result)=>{
      return result as UserProfileDto;
    }));
  }

  forgotPassword(data:ForgotPasswordDto): void{
    this.forgotPasswordMailSent$ = this.http.post(environment.baseRoute + 'User/ForgotPassword',data,{headers:this.getHeader()})
    .pipe(map((result)=>{
      return result as boolean;
    }));
  }

}
