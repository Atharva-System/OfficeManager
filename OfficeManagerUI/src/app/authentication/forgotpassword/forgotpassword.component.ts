import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { ForgotPasswordDto } from 'src/app/shared/Authentication/dtos/FrogotPasswordDto';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss']
})
export class ForgotpasswordComponent implements OnInit {

  forgotPasswordForm:FormGroup = new FormGroup({});
  forgotPassword: ForgotPasswordDto = new ForgotPasswordDto("");

  constructor(private builder:FormBuilder,private service:AuthenticationservicesService,private router:Router) { }

  ngOnInit(): void {
    this.forgotPasswordForm = this.builder.group({
      email:['',[Validators.required,Validators.email]]
    });
  }

  forgotPasswordCall()
  {
    this.forgotPassword.Email = this.forgotPasswordForm.value.email;
    this.service.forgotPassword(this.forgotPassword);
    this.service.forgotPasswordMailSent$.subscribe((result:boolean)=>
    {
      if(result)
      {
        this.router.navigateByUrl('/forgotpassword/confirmation');
      } else {
        alert("Mail sending is failed.");
      }
    })
  }

}
