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
    this.http.get(environment.baseRoute + "/SkillLevel/GetAllSkillLevel",{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        let response = res as ResponseDto;
        this._SkillLevels.next(response._Data as ISkillLevel[]);
      },
      (err)=>{
        var error = err.error as ResponseDto;
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
      }
    )
  }

  getSkillRates(): void {
    this.http.get(environment.baseRoute + "/SkillRate/GetAllSkillRate",{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        let response = res as ResponseDto;
        this._SkillRates.next(response._Data as ISkillRate[]);
      },
      (err)=>{
        var error = err.error as ResponseDto;
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
      }
    )
  }

  getSkills(search:string,pageNo:number,pageSize:number): void {
    this._Loading.next(true);
    this.http.get(environment.baseRoute + "/Skill/GetAllSkill?Search="+search+"&PageNo="
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
      },
      (err)=>{
        var error = err.error as ResponseDto;
        this._Loading.next(false);
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
        this._SkillList.next([]);
      }
    )
  }

  saveSkill(skill:SkillResponseDto){
    this.http.post(environment.baseRoute + "/Skill/AddSkill",skill,{headers:this.auth.getHeader()})
    .subscribe(
      (res)=>{
        var response = res as ResponseDto;
        if(response._StatusCode == '200'){
          this.router.navigateByUrl('/skills');
        }
      },
      (err)=>{
        var error = err.error as ResponseDto;
        if(error._Errors && error._Errors.length > 0)
          alert(error._Errors);
        else
          alert(error._Message);
      }
    )
  }
}
