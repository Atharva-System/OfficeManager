import { Component, OnInit } from '@angular/core';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { loginResponseDto } from 'src/app/shared/Authentication/dtos/loginDto';
import { UserProfileDto } from 'src/app/shared/Authentication/dtos/UserProfileDto';
import { UserRoleDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userData:UserProfileDto = new UserProfileDto("","","",new UserRoleDto("","",""),"",new Date());

  constructor(private service:AuthenticationservicesService) { }

  ngOnInit(): void {
    this.service.getUserProfile();
    this.service.userProfile$.subscribe((result:UserProfileDto)=>{
      this.userData = result;
      console.log(this.userData);
    });
  }

}
