import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllpostComponent } from './allpost/allpost.component';
import { SharedModule } from "../shared/shared.module";
import { DataTablesModule } from 'angular-datatables';



@NgModule({
  declarations: [
    AllpostComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    DataTablesModule
]
})
export class AdminModule { }
