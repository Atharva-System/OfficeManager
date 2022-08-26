import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IRolesDto } from '../../DTOs/roles-dto';
import { AuthenticationService } from '../authentication.service';
import { ResponseDto } from '../department/response-dto';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  _rolesList = new BehaviorSubject<Array<IRolesDto>>([] as IRolesDto[]);
  rolesList$ = this._rolesList.asObservable();

  constructor(private http:HttpClient,private auth:AuthenticationService) { }

  getRoles(): void{
    this.http.get(environment.baseRoute+"/UserRoles/GetAll",{headers:this.auth.getHeader()})
    .subscribe((response)=>{
      this._rolesList.next(response as IRolesDto[]);
    },
    (err)=>{
      var error = err.error as ResponseDto;
      if(error._Errors && error._Errors.length > 0)
        alert(error._Errors);
      else
        alert(error._Message);
    });
  }
}
