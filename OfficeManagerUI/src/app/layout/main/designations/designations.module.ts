import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DesignationsComponent } from './designations.component';
import { Routes, RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';

const routes: Routes = [
  {
    path:'',
    component: DesignationsComponent
  }
];

@NgModule({
  declarations: [
    DesignationsComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    RouterModule.forChild(routes)
  ]
})
export class DesignationsModule { }
