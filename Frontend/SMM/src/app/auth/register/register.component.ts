import { Component } from '@angular/core';
import { FormBuilder, FormGroup, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { CommonService } from '../../services/common/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  providers:  [ AuthserviceService ]
})
export class RegisterComponent {
  hidePwdContent: boolean = true;
  registerForm: UntypedFormGroup;
  submitted: boolean = false;
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthserviceService,
    private _commonservice: CommonService,
    private router: Router,

  ) {
    this.registerForm = fb.group({
      name: fb.control('', [Validators.required]),
      // lastName: fb.control('', [Validators.required]),
      email: fb.control('', [Validators.required]),
      phoneNumber: fb.control('', [Validators.required]),
      password: fb.control('', [Validators.required]),
      rpassword: fb.control('', [Validators.required]),
    });
  }

  register() {
    this.loading = true;
    this.submitted = true;
    if (this.registerForm.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      this.loading = false;
      return;
    }
    let user = {
      name: this.registerForm.get('name')?.value,
      phoneNumber: this.registerForm.get('phoneNumber')?.value,
      email: this.registerForm.get('email')?.value,
      // mobileNumber: this.registerForm.get('mobileNumber')?.value,
      password: this.registerForm.get('password')?.value,
    };
    this.authService.register(user).subscribe({
      next: (res) => {
        // this.open(res, 'OK');
        this.router.navigate(['/dashboard']);
        this._commonservice.successAlert("Success", "Register SuccessFul!");
        this.loading = false;
      },
    });
    this.loading = false;
  }

  get f() {
    return this.registerForm.controls;
  }

}
