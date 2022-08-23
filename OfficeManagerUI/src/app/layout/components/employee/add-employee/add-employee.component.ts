import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { debounceTime, fromEvent, merge, Observable } from 'rxjs';
import { DepartmentResponseDto } from 'src/app/shared/DTOs/department-response-dto';
import { DesignationResponseDto } from 'src/app/shared/DTOs/designation-response-dto';
import { EmployeeDetailDto, EmployeeSkill, IEmployeeDetailDto } from 'src/app/shared/DTOs/employee-list-response-dto';
import { IRolesDto } from 'src/app/shared/DTOs/roles-dto';
import { SkillLevel, SkillRate, SkillResponseDto } from 'src/app/shared/DTOs/skill-response-dto';
import { DepartmentsService } from 'src/app/shared/services/department/departments.service';
import { EmployeesService } from 'src/app/shared/services/employee/employees.service';
import { RolesService } from 'src/app/shared/services/roles/roles.service';
import { SkillsService } from 'src/app/shared/services/skills/skills.service';
import { GenericValidator } from 'src/app/shared/utility/generic-validator';
import { VALIDATION_MESSAGE } from 'src/app/shared/utility/validation-messages';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent implements OnInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  employeeForm: FormGroup = new FormGroup({});

  employeeId:number = 0;
  employeeData:EmployeeDetailDto = new EmployeeDetailDto();
  isEmployeeNoDisabled:boolean = false;

  employeeDetail$:Observable<EmployeeDetailDto> = new Observable<EmployeeDetailDto>();
  employeeSkills: EmployeeSkill[] = [];
  rolesList$:Observable<IRolesDto[]> = new Observable<IRolesDto[]>();
  departmentList$:Observable<DepartmentResponseDto[]> = new Observable<DepartmentResponseDto[]>();
  designationList$:Observable<DesignationResponseDto[]> = new Observable<DesignationResponseDto[]>();
  skillsList$:Observable<Array<SkillResponseDto>> = new Observable<Array<SkillResponseDto>>();
  skillLevelsList$:Observable<Array<SkillLevel>> = new Observable<Array<SkillLevel>>();
  skillRatesList$:Observable<Array<SkillRate>> = new Observable<Array<SkillRate>>();

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;

  constructor(private service:EmployeesService,private route:ActivatedRoute,
    private masterService:DepartmentsService,private roleService:RolesService,
    private formBuilder:FormBuilder, private datePipe:DatePipe,
    private skillService:SkillsService) {
    this.employeeDetail$ = this.service.EmployeeDetail$;
    this.genericValidator = new GenericValidator(VALIDATION_MESSAGE);
    this.skillsList$ = this.skillService.skillList$;
    this.skillLevelsList$ = this.skillService.SkillLevels$;
    this.skillRatesList$ = this.skillService.SkillRates$;
  }

  ngOnInit(): void {
    this.departmentList$ = this.masterService.DepartmentsList$;
    this.designationList$ = this.masterService.DesignationsList$;
    this.rolesList$ = this.roleService.rolesList$;
    this.skillService.getSkills('',1,1000);
    this.skillService.getSkillLevels();
    this.skillService.getSkillRates();


    this.masterService.getDepartments();
    this.masterService.getDesignations();
    this.roleService.getRoles();

    this.skillsList$.subscribe(
      (result:SkillResponseDto[])=>{
        this.employeeSkills = [];
        for(let skill of result){
          let empSkill: EmployeeSkill = new EmployeeSkill();
          empSkill.skillId = skill.id;
          empSkill.skillName = skill.name;
          empSkill.levelId = 1;
          empSkill.rateId = 1;
          this.employeeSkills.push(empSkill);
        }
      }
    )

    this.employeeForm = this.formBuilder.group({
      employeeNo:[0,[Validators.required]],
      employeeName: ['',[Validators.required,Validators.maxLength(200)]],
      email: ['',[Validators.required,Validators.maxLength(200),Validators.email]],
      dateOfJoining: [new Date(),[Validators.required]],
      dateOfBirth: [new Date(), [Validators.required]],
      designationId: [0,[Validators.required]],
      departmentId: [0,[Validators.required]],
      roleId: [0,[Validators.required]]
    })

    this.route.paramMap.subscribe((params:ParamMap)=>{
      this.employeeId = params.get('id')!= null ? Number(params.get('id')): 0;
      if(this.employeeId != 0){
        this.service.getEmployeeDetail(this.employeeId);
        this.isEmployeeNoDisabled = true;
        this.service._EmployeeDetail.subscribe(
        (response:EmployeeDetailDto) => {
          debugger
          if(response && response.employeeNo != undefined)
          {
            this.employeeForm.setValue({
              employeeNo:response.employeeNo,
              employeeName: response.employeeName,
              email: response.email,
              dateOfJoining: this.datePipe.transform(response.dateOfJoining,'yyyy-MM-dd'),
              dateOfBirth: this.datePipe.transform(response.dateOfBirth,'yyyy-MM-dd'),
              designationId: response.designationId,
              departmentId: response.departmentId,
              roleId: response.roleId
            })
          }
          for(let skill of this.employeeSkills)
          {
            debugger
            if(response.skills && response.skills.length > 0 && response.skills.filter((sk)=>sk.skillId == skill.skillId) && response.skills.filter((sk)=>sk.skillId == skill.skillId).length > 0)
            {
              skill.levelId = response.skills.filter((sk)=>sk.skillId == skill.skillId)[0].levelId;
              skill.rateId = response.skills.filter((sk)=>sk.skillId == skill.skillId)[0].rateId;

            }
            skill.employeeId = response.employeeId;
          }

          console.log(this.employeeSkills);

          this.employeeData = response;
        }
        )
      }
    })
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    merge(this.employeeForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(100)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.employeeForm);
    });
  }

  saveEmployee(): void {
    if(this.employeeForm.valid)
    {
      this.employeeData.employeeNo = this.employeeForm.value.employeeNo;
      this.employeeData.employeeName = this.employeeForm.value.employeeName;
      this.employeeData.email = this.employeeForm.value.email;
      this.employeeData.dateOfBirth = this.employeeForm.value.dateOfBirth;
      this.employeeData.dateOfJoining = this.employeeForm.value.dateOfJoining;
      this.employeeData.departmentId = this.employeeForm.value.departmentId;
      this.employeeData.designationId = this.employeeForm.value.designationId;
      this.employeeData.roleId = this.employeeForm.value.roleId;
      this.employeeData.skills = [];
      for(let skill of this.employeeSkills)
      {
        if(skill.checked == true)
          this.employeeData.skills.push(skill);
      }
    }
    debugger
    if(this.employeeData.employeeId == 0)
    {
      this.service.addEmployee(this.employeeData);
    }
    else{
      this.service.updateEmployee(this.employeeData);
    }
  }

}
