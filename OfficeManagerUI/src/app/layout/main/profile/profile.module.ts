import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile.component';
import { Routes,RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';

const routes : Routes = [
  {
    path:'',
    component: ProfileComponent
  }
]

@NgModule({
  declarations: [
    ProfileComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    RouterModule.forChild(routes)
  ]
})
export class ProfileModule { }
