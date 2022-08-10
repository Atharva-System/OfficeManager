import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginResponseDto } from '../DTOs/login-response-dto';
import { LoginModel } from '../Models/login-model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

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

  getFormDataHeader(){
    if(localStorage.getItem('token'))
    {
      return new HttpHeaders({
        'Content-Type':'application/x-www-form-urlencoded',
        'Authorization':'Bearer ' + localStorage.getItem('token')?? ''
      });
    }
    else{
      return new HttpHeaders({
        'Content-Type':'application/json'
      });
    }
  }

  constructor(private http:HttpClient,private router:Router) { }

  login(data:LoginModel):void {
    this.http.post("https://localhost:7177/api/User/Login",data).subscribe((response)=>{
      let loginResponse = response as LoginResponseDto;
      localStorage.setItem("token",loginResponse.token);
      this.router.navigateByUrl("/employees");
    });
  }
}
