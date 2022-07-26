import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForgotpasswordComponent } from 'src/app/authentication/forgotpassword/forgotpassword.component';
import { ForgotpasswordconfirmationComponent } from 'src/app/authentication/forgotpasswordconfirmation/forgotpasswordconfirmation.component';
import { LoginComponent } from 'src/app/authentication/login/login.component';
import { RegisterComponent } from 'src/app/authentication/register/register.component';
import { AuthGuard } from '../Authentication/authgaurd';

const routes: Routes = [
  {
    path:'register',
    component: RegisterComponent
  },
  {
    path:'',
    component: LoginComponent
  },
  {
    path:'forgotpassword',
    component: ForgotpasswordComponent
  },
  {
    path:'forgotpassword/confirmation',
    component: ForgotpasswordconfirmationComponent
  },
  {
    path:'main',
    loadChildren: () => import('../../layout/main/main.module').then(m => m.MainModule),
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class RouterRoutingModule { }
