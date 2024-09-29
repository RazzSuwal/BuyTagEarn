import { Component, OnInit } from '@angular/core';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { UserService } from '../../services/user/user.service';

@Component({
  selector: 'app-userpost',
  templateUrl: './userpost.component.html',
  styleUrl: './userpost.component.scss'
})
export class UserpostComponent implements OnInit {

  userId: string | null = null;
  userPosts: any[] = [];

  constructor(private authService: AuthserviceService, private userService :UserService) { }

  ngOnInit(): void {
    this.getUserID();
  }

  // Method to get the user ID
  getUserID(): void {
    this.userId = this.authService.getUserID();
    if (this.userId != null) {
      debugger
      this.getUserPost(this.userId); 
    }
    
  }
  getUserPost(userId: string): void {
    this.userService.getUserPost(userId).subscribe({
      next: (posts) => {
        debugger;
        this.userPosts = posts; 
      },
      error: (error) => {
        console.error('Error fetching user posts:', error);
      }
    });
  }
}
