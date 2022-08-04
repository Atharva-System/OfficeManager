import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { fromEvent, merge, Observable } from 'rxjs';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { DepartmentDto } from 'src/app/shared/Authentication/dtos/DepartmentDto';
import { DesignationDto } from 'src/app/shared/Authentication/dtos/DesignationDto';
import { RegisterEmployeeDto } from 'src/app/shared/Authentication/dtos/RegisterUserDto';
import { ApplicationRolesDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';
import { GenericValidator } from 'src/app/shared/utility/generic-validator';
import { PasswordMatcher } from 'src/app/shared/utility/password-matcher';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit,AfterViewInit { 
  
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  registerForm: FormGroup = new FormGroup({});
  userRoles: ApplicationRolesDto[] = [];
  departments: DepartmentDto[] = [];
  designations: DesignationDto[] = [];

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;

  constructor(private formBuilder:FormBuilder,public service:AuthenticationservicesService) {
    this.genericValidator = new GenericValidator();
  }

  ngOnInit(): void {
    this.service.getApplicationUserRoles();
    this.service.getDepartments("");
    this.service.getDesignations("");
    this.service.appRoles$.subscribe(
      (result:ApplicationRolesDto[])=>{
        this.userRoles = result;
      }
    );

    this.service.departments$.subscribe((result:DepartmentDto[])=>{
      this.departments = result;
    });

    this.service.designations$.subscribe((result:DesignationDto[])=>{
      this.designations = result;
    });

    this.registerForm = this.formBuilder.group({
      firstName:['',[Validators.required,Validators.maxLength(200)]]
      ,lastName:['',[Validators.required,Validators.maxLength(200)]]
      ,email:['',[Validators.required,Validators.email,Validators.maxLength(200)]]
      ,personalEmail:['',[Validators.required,Validators.email,Validators.maxLength(200)]]
      ,phoneNumber:['',[Validators.required,Validators.maxLength(20)]]
      ,personalPhoneNumber:['',[Validators.required,Validators.maxLength(20)]]
      ,dateOfJoining:[new Date(),[Validators.required,Validators.minLength(Date.now())]]
      ,dateOfBirth:[new Date(),[Validators.required,Validators.maxLength(Date.now())]]
      ,departmentId:['']
      ,designationId:['',[Validators.required]]
      ,roleId:['',[Validators.required]]
      ,username:['',[Validators.required,Validators.maxLength(200)]]
      ,password:['',[Validators.required,Validators.maxLength(200),Validators.minLength(8),Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,12}$')]]
      ,confirmPassword:['',[Validators.required,Validators.maxLength(200),Validators.minLength(8),Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,12}$')]]
    },{validator:PasswordMatcher.match});
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    merge(this.registerForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(100)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.registerForm);
    });
  }

  get registerFormControl() {
    return this.registerForm.controls;
  }

  registerEmployee(): void{
    if(!this.registerForm.invalid){
      var employee = new RegisterEmployeeDto("","","","","","","","","",new Date(), new Date(),"","");
      employee.FirstName = this.registerForm.value.firstName;
      employee.LastName = this.registerForm.value.lastName;
      employee.Email = this.registerForm.value.email;
      employee.PersonalEmail = this.registerForm.value.personalEmail;
      employee.PhoneNumber = this.registerForm.value.phoneNumber;
      employee.PersonalPhoneNumber = this.registerForm.value.personalPhoneNumber;
      employee.Username = this.registerForm.value.username;
      employee.Password = this.registerForm.value.password;
      employee.roleId = this.registerForm.value.roleId;
      employee.DepartmentId = this.registerForm.value.departmentId;
      employee.DesignationId = this.registerForm.value.designationId;
      employee.DateOfBirth = this.registerForm.value.dateOfBirth;
      employee.DateOfJoining = this.registerForm.value.dateOfJoining;

      this.service.addEmployee(employee);
    }
  }

}
