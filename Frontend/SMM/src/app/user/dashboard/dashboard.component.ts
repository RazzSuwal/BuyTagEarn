import { Component } from '@angular/core';
import { AuthserviceService } from '../../services/auth/authservice.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
 
 constructor(public authService: AuthserviceService,) {
  
 }
}
