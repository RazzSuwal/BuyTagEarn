import { Component } from '@angular/core';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { Router } from '@angular/router';
import { CommonService } from '../../services/common/common.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  // providers:  [ AuthserviceService]
})
export class LoginComponent {
  loginForm: UntypedFormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthserviceService,
    private router: Router,
    private _commonservice: CommonService
  ){
    
    this.loginForm = fb.group({
      email: fb.control('', [Validators.required]),
      password: fb.control('', [Validators.required])
    });
  }

  login() {
    let user = {
      userName: this.loginForm.get('email')?.value,
      password: this.loginForm.get('password')?.value,
    };
    this.authService.login(user).subscribe({
      next: (res) => {
        console.log('Login Response:', res);

        if (res && res.success && res.result && res.result.token) {
          localStorage.setItem('authToken', res.result.token);
          // Redirect or perform other actions after successful login
          this.router.navigate(['/dashboard']);
          this._commonservice.successAlert("Success",res.message);
          
        } else {
          debugger
          this._commonservice.error("UnSuccessFul",res.message);
          console.error('Invalid response structure:', res);
        }
      },
      error: (err) => {

        this._commonservice.error("UnSuccessFul",err.message);
      }
    });
  }
  
}
