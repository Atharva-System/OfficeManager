import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherforecastComponent } from './weatherforecast.component';
import { Routes,RouterModule } from '@angular/router';
import { MaterialImportsModule } from 'src/app/shared/importsModules/materialimports.module';

const routes : Routes = [
  {
    path:'',
    component: WeatherforecastComponent
  }
]

@NgModule({
  declarations: [
    WeatherforecastComponent
  ],
  imports: [
    CommonModule,
    MaterialImportsModule,
    RouterModule.forChild(routes)
  ]
})
export class WeatherforecastModule { }
