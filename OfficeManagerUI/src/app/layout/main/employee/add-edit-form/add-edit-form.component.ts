import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { DepartmentDto } from 'src/app/shared/Authentication/dtos/DepartmentDto';
import { DesignationDto } from 'src/app/shared/Authentication/dtos/DesignationDto';
import { RegisterEmployeeDto } from 'src/app/shared/Authentication/dtos/RegisterUserDto';
import { ApplicationRolesDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';

@Component({
  selector: 'app-add-edit-form',
  templateUrl: './add-edit-form.component.html',
  styleUrls: ['./add-edit-form.component.scss']
})
export class AddEditFormComponent implements OnInit {

  registerForm: FormGroup = new FormGroup({});
  userRoles: ApplicationRolesDto[] = [];
  departments: DepartmentDto[] = [];
  designations: DesignationDto[] = [];

  constructor(private formBuilder:FormBuilder,public service:AuthenticationservicesService) { }

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
      ,email:['',[Validators.required,Validators.maxLength(200)]]
      ,personalEmail:['',[Validators.required,Validators.maxLength(200)]]
      ,phoneNumber:['',[Validators.required,Validators.maxLength(200)]]
      ,personalPhoneNumber:['',[Validators.required,Validators.maxLength(200)]]
      ,dateOfJoining:['',[Validators.required,Validators.maxLength(200)]]
      ,dateOfBirth:['',[Validators.required,Validators.maxLength(200)]]
      ,departmentId:['',]
      ,designationId:['',[Validators.required,Validators.maxLength(200)]]
      ,roleId:['',[Validators.required,Validators.maxLength(200)]]
      ,username:['',[Validators.required,Validators.maxLength(200)]]
      ,password:['',[Validators.required,Validators.maxLength(200)]]
      ,confirmPassword:['',[Validators.required,Validators.maxLength(200)]]
    })
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
