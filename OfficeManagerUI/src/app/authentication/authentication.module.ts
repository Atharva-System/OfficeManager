import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationComponent } from './authentication.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { ForgotpasswordconfirmationComponent } from './forgotpasswordconfirmation/forgotpasswordconfirmation.component';
import { RouterModule, Routes } from '@angular/router';
import { MaterialImportsModule } from '../shared/importsModules/materialimports.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes : Routes = [
  {
    path:'',
    component: AuthenticationComponent,
    children:[
      {
        path:'',
        component:LoginComponent
      },
      {
        path:'register',
        component: RegisterComponent
      },
      {
        path:'forgotpassword',
        component: ForgotpasswordComponent
      }
    ]
  }
]

@NgModule({
  declarations: [
    AuthenticationComponent,
    RegisterComponent,
    LoginComponent,
    ForgotpasswordComponent,
    ForgotpasswordconfirmationComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MaterialImportsModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AuthenticationModule { }
