import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaindashboardComponent } from './maindashboard/maindashboard.component';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { AdminnavbarComponent } from './adminnavbar/adminnavbar.component';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { ChangepasswordComponent } from './changepassword/changepassword.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    MaindashboardComponent,
    NavbarComponent,
    AdminnavbarComponent,
    ChangepasswordComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    DataTablesModule,
    NgSelectModule,
    ReactiveFormsModule,
  ],
  exports: [NavbarComponent, AdminnavbarComponent]
})
export class SharedModule { }
