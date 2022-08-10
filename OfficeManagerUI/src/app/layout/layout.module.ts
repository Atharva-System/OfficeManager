import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { Routes,RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';

const routes:Routes = [
  {
    path:'',
    component: LayoutComponent,
    children: [
      {
        path:'employees',
        loadChildren: () => import('./components/employee/employee.module').then(m => m.EmployeeModule)
      }
    ]
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
    RouterModule.forChild(routes)
  ]
})
export class LayoutModule { }
