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

  constructor(public authService: AuthserviceService,
    private router: Router,
    private _commonservice: CommonService) {


  }
  ngOnInit(): void {
    this.getUserRole();
  }

  getUserRole() {
    this.authService.getUserRole().subscribe(role => {
      this.userRole= role;

    });
    console.log(this.userRole);

  }
}
