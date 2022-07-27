import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { fromEvent, merge, Observable,debounceTime } from 'rxjs';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { LoginDto } from 'src/app/shared/Authentication/dtos/loginDto';
import { GenericValidator } from 'src/app/shared/utility/generic-validator';
import { LOGIN_VALIDATION_MESSAGE } from 'src/app/shared/utility/validation-messages';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  loginForm: FormGroup = new FormGroup({});
  login:LoginDto = new LoginDto("","")

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;

  constructor(private formBuilder:FormBuilder,public service:AuthenticationservicesService) {
    this.genericValidator = new GenericValidator(LOGIN_VALIDATION_MESSAGE);
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username:['',[Validators.required,Validators.maxLength(200)]],
      password:['',[Validators.required,Validators.minLength(8),Validators.maxLength(200)]],
    })
  }
  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    merge(this.loginForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(100)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.loginForm);
    });
  }

  loginUser() : void {
    if(this.loginForm.valid)
    {
      this.login.UserName = this.loginForm.value.username;
      this.login.Password = this.loginForm.value.password;
      this.service.loginUser(this.login);
    }
    else{
      alert("Please check the data there is some issue.");
    }
  }

}
