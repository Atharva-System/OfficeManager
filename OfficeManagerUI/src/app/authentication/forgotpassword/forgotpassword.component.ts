import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fromEvent, merge, Observable, debounceTime } from 'rxjs';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { ForgotPasswordDto } from 'src/app/shared/Authentication/dtos/FrogotPasswordDto';
import { GenericValidator } from 'src/app/shared/utility/generic-validator';
import { FORGOTPASSWORD_VALIDATION_MESSAGE } from 'src/app/shared/utility/validation-messages';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss']
})
export class ForgotpasswordComponent implements OnInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  forgotPasswordForm:FormGroup = new FormGroup({});
  forgotPassword: ForgotPasswordDto = new ForgotPasswordDto("");

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;

  constructor(private builder:FormBuilder,private service:AuthenticationservicesService,private router:Router) {
    this.genericValidator = new GenericValidator(FORGOTPASSWORD_VALIDATION_MESSAGE);
  }

  ngOnInit(): void {
    this.forgotPasswordForm = this.builder.group({
      email:['',[Validators.required,Validators.email]]
    });
  }

  ngAfterViewInit(): void {
    type NewType = Observable<any>;

    // Watch for the blur event from any input element on the form.
    const controlBlurs: NewType[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    merge(this.forgotPasswordForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(100)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.forgotPasswordForm);
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
