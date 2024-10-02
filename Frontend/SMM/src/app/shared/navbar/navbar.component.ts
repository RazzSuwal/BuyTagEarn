import { Component, OnInit } from '@angular/core';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { Router } from '@angular/router';
import { CommonService } from '../../services/common/common.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'] // Fixed typo: 'styleUrl' should be 'styleUrls'
})
export class NavbarComponent implements OnInit {
  isAdminUser: boolean = false; // Property to store the admin status

  constructor(
    public authService: AuthserviceService,
    private router: Router,
    private _commonservice: CommonService
  ) { }

  ngOnInit() {
    // this.isAdmin();
  }

  isAdmin() {
    this.authService.getUserRole().subscribe(role => {
      this.isAdminUser = (role === 'admin'); // Update the property based on role
    });
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this._commonservice.successAlert("Success", "Logout Successful!");
  }
}


  // isUser(): boolean {
  //   return this.authService.getUserRole() === 'user';
  // }

