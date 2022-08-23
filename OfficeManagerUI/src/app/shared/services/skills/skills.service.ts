import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResponse,IPaginatedResponse } from '../../DTOs/paginated-response-dto';
import { SkillResponseDto,ISkillResponseDto, SkillLevel, ISkillLevel, ISkillRate, SkillRate } from '../../DTOs/skill-response-dto';
import { AuthenticationService } from '../authentication.service';
import { ResponseDto } from '../department/response-dto';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {

  _SkillList = new BehaviorSubject<Array<SkillResponseDto>>([]);
  skillList$ = this._SkillList.asObservable();

  _PaginatedList = new BehaviorSubject<IPaginatedResponse<ISkillResponseDto>>({} as IPaginatedResponse<ISkillResponseDto>);
  PaginatedList$ = this._PaginatedList.asObservable();

  _Loading = new BehaviorSubject<boolean>(false);
  Loading$ = this._Loading.asObservable();

  _SkillLevels = new BehaviorSubject<ISkillLevel[]>([] as ISkillLevel[]);
  SkillLevels$ = this._SkillLevels.asObservable();

  _SkillRates = new BehaviorSubject<ISkillRate[]>([] as ISkillRate[]);
  SkillRates$ = this._SkillRates.asObservable();

  constructor(private http:HttpClient,private auth:AuthenticationService,private router:Router) { }

  getSkillLevels(): void{
    this.http.get(environment.baseRoute + "/Masters/SkillLevels",{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        let response = res as ResponseDto;
        this._SkillLevels.next(response._Data as ISkillLevel[]);
      }
    )
  }

  getSkillRates(): void {
    this.http.get(environment.baseRoute + "/Masters/SkillRates",{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        let response = res as ResponseDto;
        this._SkillRates.next(response._Data as ISkillRate[]);
      }
    )
  }

  getSkills(search:string,pageNo:number,pageSize:number): void {
    this._Loading.next(true);
    this.http.get(environment.baseRoute + "/Masters/Skill?Search="+search+"&PageNo="
    +pageNo+"&PageSize="+pageSize,{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        var response = res as ResponseDto;
        this._PaginatedList.next(response._Data as PaginatedResponse<SkillResponseDto>)
        this._PaginatedList.subscribe(
          (result)=>{
            this._SkillList.next(result.items as ISkillResponseDto[]);
            this._Loading.next(false);
          }
        )
      }
    )
  }

  saveSkill(skill:SkillResponseDto){
    this.http.post("https://localhost:7177/api/Masters/Skill/Add",skill,{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        var response = res as ResponseDto;
        if(response._StatusCode == '200'){
          this.router.navigateByUrl('/skills');
        }
      }
    )
  }
}
