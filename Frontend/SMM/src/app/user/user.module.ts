import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactusComponent } from './contactus/contactus.component';
import { RouterModule } from '@angular/router';
import { MainFormComponent } from './main-form/main-form.component';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UserpostComponent } from './userpost/userpost.component';
import { UserpostDetailsComponent } from './userpost-details/userpost-details.component';
import { NgSelectModule } from '@ng-select/ng-select';



@NgModule({
  declarations: [
    DashboardComponent,
    ContactusComponent,
    MainFormComponent,
    UserpostComponent,
    UserpostDetailsComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    ReactiveFormsModule,
    NgSelectModule
  ],
  exports: [
    DashboardComponent  // Export the DashboardComponent
  ]
})
export class UserModule { }
