import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillsComponent } from './skills.component';
import { RouterModule, Routes } from '@angular/router';
import { AddeditComponent } from './addedit/addedit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReactiveComponentModule } from '@ngrx/component';

const routes:Routes = [
  {
    path:'',
    component:SkillsComponent
  },
  {
    path:'add',
    component: AddeditComponent
  }
]

@NgModule({
  declarations: [
    SkillsComponent,
    AddeditComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ReactiveComponentModule,
    RouterModule.forChild(routes)
  ]
})
export class SkillsModule { }
