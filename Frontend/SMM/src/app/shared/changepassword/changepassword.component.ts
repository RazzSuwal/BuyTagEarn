import { Component } from '@angular/core';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CommonService } from '../../services/common/common.service';
import { AuthserviceService } from '../../services/auth/authservice.service';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrl: './changepassword.component.scss'
})
export class ChangepasswordComponent {

  changePasswordFrom: UntypedFormGroup;
  submitted: boolean = false;
  userEmail: any;

  constructor(
    private fb: FormBuilder,
    private _commonservice: CommonService,
    private _authService: AuthserviceService,
  ){
    this.changePasswordFrom = fb.group({
      oldPassword: fb.control('', [Validators.required]),
      newPassword: fb.control('', [Validators.required]),
      email: fb.control(null),
    });
  }

  submit() {
    this.submitted = true;
    if (this.changePasswordFrom.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      return;
    }
    this._authService.changePassword(this.userEmail, this.changePasswordFrom.get('oldPassword')?.value, this.changePasswordFrom.get('newPassword')?.value).subscribe({
      next: (res) => {
        this._commonservice.successAlert("Success", "Submit SuccessFul!");
        this.changePasswordFrom.reset();

      },
    });
  }

  getUserDetails() {
    const userDetails = this._authService.getUserDetails();
    if (userDetails) {
      this.userEmail = userDetails;
    } else {
      this.userEmail = null;
    }
  }

  get f() {
    return this.changePasswordFrom.controls;
  }
}
