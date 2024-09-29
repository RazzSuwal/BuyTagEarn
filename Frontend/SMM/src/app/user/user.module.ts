import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactusComponent } from './contactus/contactus.component';
import { RouterModule } from '@angular/router';
import { MainFormComponent } from './main-form/main-form.component';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UserpostComponent } from './userpost/userpost.component';



@NgModule({
  declarations: [
    DashboardComponent,
    ContactusComponent,
    MainFormComponent,
    UserpostComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    ReactiveFormsModule
  ],
  exports: [
    DashboardComponent  // Export the DashboardComponent
  ]
})
export class UserModule { }
