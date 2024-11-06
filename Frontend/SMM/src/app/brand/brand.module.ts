import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { UserPaymentRequestComponent } from './user-payment-request/user-payment-request.component';
import { DataTablesModule } from 'angular-datatables';
import { ProductComponent } from './product/product.component';



@NgModule({
  declarations: [
    UserPaymentRequestComponent,
    ProductComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    ReactiveFormsModule,
    DataTablesModule
  ]
})
export class BrandModule { }
