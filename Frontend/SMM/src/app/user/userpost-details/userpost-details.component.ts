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
  showAskPayment: any;
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
        this.PaymentFunction(res[0].PostedOn)
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
  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getCurrentDate(): string {
    const now = new Date();

    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
  }

  PaymentFunction(postedDate: string) {
    debugger
    const currentDate = new Date(this.getCurrentDate());
    const postedDates = new Date(this.getNextTwoMonths(postedDate));

    if (postedDates < currentDate) {
      this.showAskPayment = true;
    }else{
      this.showAskPayment = false;
    }
  }

  getNextTwoMonths(dateString: string): string {
    const date = new Date(dateString);

    date.setMonth(date.getMonth() + 2);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
  }

}
