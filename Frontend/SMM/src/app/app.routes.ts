import { Routes } from '@angular/router';
import { DashboardComponent } from './user/dashboard/dashboard.component';
import { ContactusComponent } from './user/contactus/contactus.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { MaindashboardComponent } from './shared/maindashboard/maindashboard.component';
import { authGuard } from './services/auth/auth.guard';
import { MainFormComponent } from './user/main-form/main-form.component';
import { UserpostComponent } from './user/userpost/userpost.component';
import { AllpostComponent } from './admin/allpost/allpost.component';
import { UserpostDetailsComponent } from './user/userpost-details/userpost-details.component';
import { UserPaymentRequestComponent } from './brand/user-payment-request/user-payment-request.component';
import { ProductComponent } from './brand/product/product.component';
import { BrandDetailsComponent } from './admin/brand-details/brand-details.component';
import { BrandproductsComponent } from './admin/brandproducts/brandproducts.component';
import { UserEarnComponent } from './user/user-earn/user-earn.component';
import { ChangepasswordComponent } from './shared/changepassword/changepassword.component';

export const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'contactus', component: ContactusComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: MaindashboardComponent, canActivate: [authGuard] },
    { path: 'main-form', component: MainFormComponent, canActivate: [authGuard] },
    { path: 'mypost', component: UserpostComponent, canActivate: [authGuard] },
    { path: 'allpost', component: AllpostComponent, canActivate: [authGuard] },
    { path: 'postDetails/:id', component: UserpostDetailsComponent, canActivate: [authGuard] },
    { path: 'userPaymentRequest', component: UserPaymentRequestComponent, canActivate: [authGuard] },
    { path: 'product', component: ProductComponent, canActivate: [authGuard] },
    { path: 'brand', component: BrandDetailsComponent, canActivate: [authGuard] },
    { path: 'brandProduct', component: BrandproductsComponent, canActivate: [authGuard] },
    { path: 'user-earn', component: UserEarnComponent, canActivate: [authGuard] },
    { path: 'changePassword', component: ChangepasswordComponent, canActivate: [authGuard] },
];
