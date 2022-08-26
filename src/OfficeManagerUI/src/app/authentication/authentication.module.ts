import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationComponent } from './authentication.component';
import { Routes,RouterModule } from '@angular/router';

const routes:Routes = [
  {
    path:'',
    component: AuthenticationComponent,
    children:[
      {
        path:'login',
        loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
      }
    ]
  }
]

@NgModule({
  declarations: [
    AuthenticationComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class AuthenticationModule { }
