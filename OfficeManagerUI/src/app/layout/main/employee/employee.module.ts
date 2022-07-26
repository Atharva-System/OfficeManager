import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddEditFormComponent } from './add-edit-form/add-edit-form.component';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeComponent } from './employee.component';

const routes: Routes = [
  {
    path:'',
    component: EmployeeComponent
  }
];

@NgModule({
  declarations: [
    EmployeeComponent,
    AddEditFormComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class EmployeeModule { }
