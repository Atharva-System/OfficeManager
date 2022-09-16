import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { Routes,RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { AuthGuard } from '../shared/utility/auth-guard';
import { ReactiveComponentModule } from '@ngrx/component';

const routes:Routes = [
  {
    path:'',
    component: LayoutComponent,
    children: [
      {
        path:'employees',
        loadChildren: () => import('./components/employee/employee.module').then(m => m.EmployeeModule),
        canActivate:[AuthGuard]
      },
      {
        path: 'skills',
        loadChildren: () => import('./components/skills/skills.module').then(m => m.SkillsModule),
        canActivate: [AuthGuard]
      }
    ],
    canActivate:[AuthGuard]
  }
]

@NgModule({
  declarations: [
    LayoutComponent,
    SidebarComponent,
    HeaderComponent
  ],
  imports: [
    CommonModule,
    ReactiveComponentModule,
    RouterModule.forChild(routes)
  ]
})
export class LayoutModule { }
