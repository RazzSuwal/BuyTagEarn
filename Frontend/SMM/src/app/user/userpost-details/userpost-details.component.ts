import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user/user.service';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CommonService } from '../../services/common/common.service';
import { PaymentService } from '../../services/payment/payment.service';

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
  paymentRequestForm: UntypedFormGroup;
  submitted: boolean = false;
  imageSrc: any;
  amount: any;
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private _userservice: UserService,
    private _commonservice: CommonService,
    private _payment: PaymentService,
    private router: Router,
  ) {
    this.paymentRequestForm = fb.group({
      mobileNo: fb.control('', [Validators.required]),
      userPostId: fb.control(null),
      amount: fb.control(null),
      requestId: fb.control(null),
    });
  }
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
        this.postDetails = res;
        if (this.postDetails.ImageBase64) {
          this.imageSrc = `data:image/jpeg;base64,${this.postDetails.ImageBase64}`;
        }
        this.getEmbedLikesAndUrl(res.PostUrl);
        this.PaymentFunction(res.PostedOn);
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
        this.amount = String(this.postLikes * 100);
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
    debugger;
    const currentDate = new Date(this.getCurrentDate());
    const postedDates = new Date(this.getNextTwoMonths(postedDate));

    this.showAskPayment = true;
    // if (postedDates < currentDate) {
    //   this.showAskPayment = true;
    // } else {
    //   this.showAskPayment = false;
    // }
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

  submit() {
    this.submitted = true;
    if (this.paymentRequestForm.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      return;
    }
    debugger
    let data = {
      mobileNo: this.paymentRequestForm.get('mobileNo')?.value,
      userPostId: this.postDetails.UserPostId,
      amount: this.amount,
      
    };
    this._payment.paymentRequest(data).subscribe({
      next: (res) => {
        this._commonservice.successAlert("Success", "Submit SuccessFul!");
        this.paymentRequestForm.reset();
        this.submitted = false;
        //need to work on
        // $('#exampleModal').modal('hide');

      },
    });
  }

  get f() {
    return this.paymentRequestForm.controls;
  }
}
