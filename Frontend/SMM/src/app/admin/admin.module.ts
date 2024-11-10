import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllpostComponent } from './allpost/allpost.component';
import { SharedModule } from "../shared/shared.module";
import { DataTablesModule } from 'angular-datatables';
import { BrandDetailsComponent } from './brand-details/brand-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrandproductsComponent } from './brandproducts/brandproducts.component';



@NgModule({
  declarations: [
    AllpostComponent,
    BrandDetailsComponent,
    BrandproductsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    DataTablesModule
]
})
export class AdminModule { }
