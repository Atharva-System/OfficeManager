import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { EmployeeComponent } from './employee.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReactiveComponentModule } from '@ngrx/component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { ImportEmployeeComponent } from './import-employee/import-employee.component';

const routes: Routes = [
  {
    path:'',
    component:EmployeeComponent
  },
  {
    path:'add',
    component: AddEmployeeComponent
  },
  {
    path:'edit/:id',
    component: AddEmployeeComponent
  }
]

@NgModule({
  declarations: [
    EmployeeComponent,
    AddEmployeeComponent,
    ImportEmployeeComponent,
  ],
  imports: [
    CommonModule,
    ReactiveComponentModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ],
  providers:[
    DatePipe
  ]
})
export class EmployeeModule { }
