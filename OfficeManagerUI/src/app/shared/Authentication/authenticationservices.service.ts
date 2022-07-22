import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterUserDto } from './dtos/RegisterUserDto';
import { UserRoleDto } from './dtos/UserRoleDto';
import { LoginDto, loginResponseDto } from './dtos/loginDto';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { BehaviorSubject, map } from 'rxjs';
import { WeatherForecastDto } from './dtos/WeatherForecastDto';
import { UserProfileDto } from './dtos/UserProfileDto';

const BASE_ROUTE = 'https://localhost:7177/api/'

@Injectable({
  providedIn: 'root'
})
export class AuthenticationservicesService {

  registerUserData:RegisterUserDto = new RegisterUserDto("","","","",new Date(),"");

  roles:UserRoleDto[] = [];
  _role = new BehaviorSubject<UserRoleDto[]>([]);
  role$ = this._role.asObservable();

  _weatherForecast = new BehaviorSubject<WeatherForecastDto[]>([]);
  weatherForecast$ = this._weatherForecast.asObservable();

  _userProfile = new BehaviorSubject<UserProfileDto>(new UserProfileDto("","","",new UserRoleDto("","",""),"",new Date()));
  userProfile$ = this._userProfile.asObservable();

  loginResponse: loginResponseDto = new loginResponseDto("","","","","");

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

  registerUser(): void{
    this.http.post("https://localhost:7177/api/User/Register",this.registerUserData).subscribe((result)=>
    {
      console.log(result);
    });
  }

  getUserRoles(): void {
    this.role$ = this.http.get("https://localhost:7177/api/Roles/")
    .pipe(map((result:any)=>{
      return result as UserRoleDto[];
    }));
  }

  editRole(data:UserRoleDto)
  {
    this.http.put(BASE_ROUTE + "Roles/Edit/",data,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Role updated successfully");
      this.getUserRoles();
    })
  }

  deleteRole(id:string):any{
    return this.http.delete(BASE_ROUTE + "Roles/Delete/"+id,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Role deleted successfully");
      this.getUserRoles();
      return true;
    },
    (error)=>{
      this.toastr.error("Something went wrong.");
      return false;
    })
  }

  addRole(data:UserRoleDto)
  {
    this.http.post(BASE_ROUTE + "Roles/Add/",data,{headers:this.getHeader()}).subscribe((result)=>{
      this.toastr.success("Role added successfully");
      this.getUserRoles();
    })
  }

  loginUser(data:LoginDto)
  {
    this.http.post("https://localhost:7177/api/User/Login",data,{headers:this.getHeader()})
    .subscribe((result)=>{
      this.loginResponse = result as loginResponseDto;
      localStorage.setItem("token",this.loginResponse.token);
      this.toastr.success("User logged in successfully.");
      this.router.navigate(['/main']);
    })
  }

  getWeatherForecast()
  {
    this.weatherForecast$ = this.http.get(BASE_ROUTE + "WeatherForecast/All",{headers:this.getHeader()}).pipe(map((result)=>{
      return result as WeatherForecastDto[];
    }));
  }

  getUserProfile()
  {
    debugger
    this.userProfile$ = this.http.get(BASE_ROUTE + "User/"+this.loginResponse.userId,{headers:this.getHeader()}).pipe(map((result)=>{
      return result as UserProfileDto;
    }));
  }

}
