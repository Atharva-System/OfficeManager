import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  current_page$:Observable<string> = new Observable<string>();

  constructor(private router:Router,private auth:AuthenticationService) {
    this.current_page$ = this.auth.Header$;
  }

  ngOnInit(): void {
  }

  logout(){
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }

}
