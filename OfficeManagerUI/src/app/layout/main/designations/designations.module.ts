import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DesignationsComponent } from './designations.component';
import { Routes, RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';
import { CreateComponent } from './create/create.component';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path:'',
    component: DesignationsComponent
  }
];

@NgModule({
  declarations: [
    DesignationsComponent,
    CreateComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class DesignationsModule { }
