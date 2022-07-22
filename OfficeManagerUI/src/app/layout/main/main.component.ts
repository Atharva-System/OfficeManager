import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { loginResponseDto } from 'src/app/shared/Authentication/dtos/loginDto';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  open = true;
  email = ''

  constructor(private service:AuthenticationservicesService,private router:Router) { }

  ngOnInit(): void {
    this.email = this.service.loginResponse.email
  }

  signout()
  {
     localStorage.clear();
     this.service.loginResponse = new loginResponseDto("","","","","");
     this.router.navigate(['/login']);
  }

}
