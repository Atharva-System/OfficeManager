import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { fromEvent, merge, Observable,debounceTime } from 'rxjs';
import { LoginModel } from 'src/app/shared/Models/login-model';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
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
  login:LoginModel = new LoginModel();

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;

  constructor(private formBuilder:FormBuilder,public service:AuthenticationService) {
    this.genericValidator = new GenericValidator(LOGIN_VALIDATION_MESSAGE);
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      employeeNo:[0,[Validators.required]],
      password:['',[Validators.required,Validators.minLength(8),Validators.maxLength(200)]]
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

  loginUser():void {
    if(this.loginForm.valid)
    {
      this.login.EmployeeNo = this.loginForm.value.employeeNo;
      this.login.Password = this.loginForm.value.password;
      this.service.login(this.login);
    } else {
      alert("Please check your credentials!")
    }
  }
}
