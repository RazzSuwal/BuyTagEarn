import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaindashboardComponent } from './maindashboard/maindashboard.component';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { AdminnavbarComponent } from './adminnavbar/adminnavbar.component';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';



@NgModule({
  declarations: [
    MaindashboardComponent,
    NavbarComponent,
    AdminnavbarComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    DataTablesModule,
    NgSelectModule
  ],
  exports: [NavbarComponent, AdminnavbarComponent]
})
export class SharedModule { }
