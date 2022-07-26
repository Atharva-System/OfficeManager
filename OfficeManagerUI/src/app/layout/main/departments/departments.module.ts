import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepartmentsComponent } from './departments.component';
import { Routes, RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';
import { CreateComponent } from './create/create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path:'',
    component: DepartmentsComponent
  }
];


@NgModule({
  declarations: [
    DepartmentsComponent,
    CreateComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class DepartmentsModule { }
