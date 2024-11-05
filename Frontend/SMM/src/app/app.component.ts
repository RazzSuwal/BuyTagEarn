import { Component, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserModule } from './user/user.module';
import { AuthModule } from './auth/auth.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { SharedModule } from './shared/shared.module';
import { AdminModule } from './admin/admin.module';
import { DataTablesModule } from 'angular-datatables';
import { BrandModule } from './brand/brand.module';

@Component({
  selector: 'app-root',
  standalone: true,
  encapsulation: ViewEncapsulation.None,
  imports: [
    RouterOutlet,
    UserModule,
    AuthModule,
    SharedModule,
    HttpClientModule,
    ReactiveFormsModule,
    AdminModule,
    BrandModule,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS }, // Provide JWT_OPTIONS if used
    JwtHelperService, // Provide JwtHelperService
  ]
})
export class AppComponent {
  title = 'SMM';
}
