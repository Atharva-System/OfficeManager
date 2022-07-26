import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { LoginDto } from 'src/app/shared/Authentication/dtos/loginDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({});
  login:LoginDto = new LoginDto("","")
  constructor(private formBuilder:FormBuilder,public service:AuthenticationservicesService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username:['',[Validators.email,Validators.required]],
      password:['',[Validators.required]],
    })
  }

  loginUser() : void {
    this.login.UserName = this.loginForm.value.username;
    this.login.Password = this.loginForm.value.password;
    this.service.loginUser(this.login);
  }

}
