import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeComponent } from './employee.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReactiveComponentModule } from '@ngrx/component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';

const routes: Routes = [
  {
    path:'',
    component:EmployeeComponent
  },
  {
    path:'add',
    component: AddEmployeeComponent
  }
]

@NgModule({
  declarations: [
    EmployeeComponent,
    AddEmployeeComponent,
  ],
  imports: [
    CommonModule,
    ReactiveComponentModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class EmployeeModule { }
