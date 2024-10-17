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

export const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'contactus', component: ContactusComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: MaindashboardComponent, canActivate: [authGuard] },
    { path: 'main-form', component: MainFormComponent, canActivate: [authGuard] },
    { path: 'mypost', component: UserpostComponent, canActivate: [authGuard] },
    { path: 'allpost', component: AllpostComponent, canActivate: [authGuard] },
    
    
];
