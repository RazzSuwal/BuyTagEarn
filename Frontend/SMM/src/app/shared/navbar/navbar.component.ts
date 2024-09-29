import { Component } from '@angular/core';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { Router } from '@angular/router';
import { CommonService } from '../../services/common/common.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  constructor(public authService: AuthserviceService, private router: Router, private _commonservice: CommonService) { }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this._commonservice.successAlert("Success", "Logout SuccessFul!");
  }

  isAdmin(): boolean {
    return this.authService.getUserRole() === 'admin';
  }

  isUser(): boolean {
    return this.authService.getUserRole() === 'user';
  }
}
