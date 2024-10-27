import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../services/user/user.service';

@Component({
  selector: 'app-userpost-details',
  templateUrl: './userpost-details.component.html',
  styleUrl: './userpost-details.component.scss',
})
export class UserpostDetailsComponent {
  postDetails: any;
  postId: number = 0;
  postLikes: any;
  postImgUrl: any;
  constructor(
    private route: ActivatedRoute,
    private _userservice: UserService
  ) {}
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.postId = +params['id'];
      console.log(this.postId);
    });
    this.getUserPostDetails();
  }
  getUserPostDetails() {
    this._userservice.getUserPostsDetails(this.postId).subscribe(
      (res: any) => {
        this.postDetails = res[0];
        this.getEmbedLikesAndUrl(res[0].PostUrl)
      },
      (error: any) => {
        console.error('Error fetching data:', error);
      }
    );
  }

  getEmbedLikesAndUrl(postUrl: string) {
    this._userservice.getEmbedLikesAndUrl(postUrl).subscribe(
      (res: any) => {
        console.log(res);
        this.postLikes = res.likes;
        this.postImgUrl = res.imageUrl;

      },
      (error: any) => {
        console.error('Error fetching data:', error);
      }
    );
  }
}
