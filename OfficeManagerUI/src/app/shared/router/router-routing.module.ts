import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../Authentication/authgaurd';

const routes: Routes = [
  {
    path:'',
    loadChildren: () => import('../../authentication/authentication.module').then(m => m.AuthenticationModule)
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
