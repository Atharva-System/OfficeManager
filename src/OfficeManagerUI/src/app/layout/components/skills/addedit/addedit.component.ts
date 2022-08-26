import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { fromEvent, merge, Observable, debounceTime } from 'rxjs';
import { SkillResponseDto } from 'src/app/shared/DTOs/skill-response-dto';
import { SkillsService } from 'src/app/shared/services/skills/skills.service';
import { GenericValidator } from 'src/app/shared/utility/generic-validator';
import { SKILL_MASTER_VALIDATION_MESSAGE } from 'src/app/shared/utility/validation-messages';

@Component({
  selector: 'app-addedit',
  templateUrl: './addedit.component.html',
  styleUrls: ['./addedit.component.scss']
})
export class AddeditComponent implements OnInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  skillForm: FormGroup = new FormGroup({});
  skill:SkillResponseDto = new SkillResponseDto();

  // Use with the generic validation message class
  displayMessage: { [key: string]: string } = {};
  private genericValidator:GenericValidator;
  constructor(private formBuilder:FormBuilder,private service:SkillsService) {
    this.genericValidator = new GenericValidator(SKILL_MASTER_VALIDATION_MESSAGE);
  }

  ngOnInit(): void {
    this.skillForm = this.formBuilder.group({
      id:[0],
      title:['',[Validators.required]],
      description:[''],
      isActive:[true],
    })
  }



  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    merge(this.skillForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(100)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.skillForm);
    });
  }

  saveSkill(): void{
    if(this.skillForm.valid)
    {
      this.skill.id = this.skillForm.value.id;
      this.skill.name = this.skillForm.value.title;
      this.skill.description = this.skillForm.value.description;
      this.skill.isActive = this.skillForm.value.isActive;
      this.service.saveSkill(this.skill);
    }
  }

}
