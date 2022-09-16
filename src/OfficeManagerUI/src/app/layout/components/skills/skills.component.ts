import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IPaginatedResponse } from 'src/app/shared/DTOs/paginated-response-dto';
import { ISkillResponseDto, SkillResponseDto } from 'src/app/shared/DTOs/skill-response-dto';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { SkillsService } from 'src/app/shared/services/skills/skills.service';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss']
})
export class SkillsComponent implements OnInit {

  search = "";
  pageNo = 1;
  pageSize = 10;

  loading$:Observable<boolean> = new Observable<boolean>();
  skillPage$:Observable<IPaginatedResponse<ISkillResponseDto>> = new Observable<IPaginatedResponse<ISkillResponseDto>>();
  skills$: Observable<Array<ISkillResponseDto>> = new Observable<Array<ISkillResponseDto>>();

  constructor(private service:SkillsService,private auth:AuthenticationService) {
    this.loading$ = this.service.Loading$;
    this.skillPage$ = this.service.PaginatedList$;
    this.skills$ = this.service.skillList$;
    this.pageNo = 1;
  }

  ngOnInit(): void {
    this.service.getSkills('',1,10);
    this.auth.Header = "Skill";
  }

  searchSkill(): void {
    this.service.getSkills(this.search,this.pageNo,this.pageSize);
  }

  createRange(number:number){
    // return new Array(number);
    return new Array(number).fill(0)
      .map((n, index) => index + 1);
  }

  paginate(pageNumber:number): void{
    this.pageNo = pageNumber;
    this.service.getSkills(this.search,this.pageNo,this.pageSize);
  }

}
