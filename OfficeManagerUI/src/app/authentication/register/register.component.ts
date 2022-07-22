import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup = new FormGroup({});

  constructor(private formBuilder:FormBuilder,public service:AuthenticationservicesService) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email:['',[Validators.email,Validators.required]],
      password:['',[Validators.required]],
      confirmPassword:['',[Validators.required]],
      contact:['',[Validators.required]],
      dateOfJoining:['',[Validators.required]],
      personalEmail:['',[Validators.email,Validators.required]],
      role:['',[Validators.required]]
    })

    this.service.getUserRoles();  
  }

  registerUser()
  {
    this.service.registerUserData.Email = this.registerForm.value.email;
    this.service.registerUserData.Password = this.registerForm.value.password;
    this.service.registerUserData.Contact = this.registerForm.value.contact;
    this.service.registerUserData.PersonalEmail = this.registerForm.value.personalEmail;
    this.service.registerUserData.DateOfJoining = this.registerForm.value.dateOfJoining;
    this.service.registerUserData.RoleId = this.registerForm.value.role;

    this.service.registerUser();
  }

}
