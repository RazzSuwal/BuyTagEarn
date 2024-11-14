import { Component, OnInit } from '@angular/core';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { Router } from '@angular/router';
import { CommonService } from '../../services/common/common.service';

@Component({
  selector: 'app-adminnavbar',
  templateUrl: './adminnavbar.component.html',
  styleUrl: './adminnavbar.component.scss'
})
export class AdminnavbarComponent implements OnInit{
  userRole: any;
  userEmail!:any;
  isDropdownOpen: boolean = false;
  userName: string = '';
  userAvatar: string = '/assets/images/UserProfileImage.jpg';

  constructor(public authService: AuthserviceService,
    private router: Router,
    private _commonservice: CommonService,
  ) {


  }
  ngOnInit(): void {
    this.getUserRole();
    this.getUserDetails();
  }

  getUserRole() {
    this.authService.getUserRole().subscribe(role => {
      this.userRole= role;

    });

  }
  onLogout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this._commonservice.successAlert("Success", "Logout Successful!");
  }

  getUserDetails() {
    const userDetails = this.authService.getUserDetails();
    if (userDetails) {
      this.userEmail = userDetails;
      this.userName = userDetails ? userDetails.split('@')[0] : 'Guest';
    } else {
      this.userEmail = null;
    }
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }


}
