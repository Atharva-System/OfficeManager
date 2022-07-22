import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from 'src/app/authentication/login/login.component';
import { RegisterComponent } from 'src/app/authentication/register/register.component';
import { AuthGuard } from '../Authentication/authgaurd';

const routes: Routes = [
  {
    path:'',
    component: RegisterComponent
  },
  {
    path:'login',
    component: LoginComponent
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
