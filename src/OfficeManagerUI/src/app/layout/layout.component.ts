import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../shared/services/authentication.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  Loading$ = new Observable<boolean>();

  constructor(private auth:AuthenticationService) {
    this.Loading$ = this.auth.Loading$;
  }

  ngOnInit(): void {
  }

}
