import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RolesComponent } from './roles.component';
import { Routes, RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';
import { AddroleComponent } from './addrole/addrole.component';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path:'',
    component: RolesComponent
  }
];


@NgModule({
  declarations: [
    RolesComponent,
    AddroleComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class RolesModule { }
