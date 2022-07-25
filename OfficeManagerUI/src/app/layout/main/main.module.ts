import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { RouterModule, Routes } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';
import { AuthGuard } from 'src/app/shared/Authentication/authgaurd';
import { ProfileComponent } from './profile/profile.component';
import { EmployeeComponent } from './employee/employee.component';

const routes: Routes = [
  {
    path:'',
    component:MainComponent,
    children:[
      {
        path:'',
        loadChildren: () => import("./roles/roles.module").then(m => m.RolesModule)
      },
      {
        path:'department',
        loadChildren: () => import('./departments/departments.module').then(m => m.DepartmentsModule)
      },
      {
        path:'designation',
        loadChildren: () => import('./designations/designations.module').then(m => m.DesignationsModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule)
      },
      {
        path: 'weatherforecast',
        loadChildren: () => import('./weatherforecast/weatherforecast.module').then(m => m.WeatherforecastModule)
      }
    ],
    canActivate: [AuthGuard]
  },
  
]

@NgModule({
  declarations: [
    MainComponent,
    EmployeeComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    RouterModule.forChild(routes)
  ],
  providers: [AuthGuard]
})
export class MainModule { }
