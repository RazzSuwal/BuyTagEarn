import { Component } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { PaymentService } from '../../services/payment/payment.service';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-user-payment-request',
  templateUrl: './user-payment-request.component.html',
  styleUrl: './user-payment-request.component.scss'
})
export class UserPaymentRequestComponent {
  dtOptions: Config={};
  dtTrigger: Subject<any> = new Subject<any>();
  requestData!: any[];
  ssForm: UntypedFormGroup;
  submitted: boolean = false;
  selectedFile: File | null = null;
  id: any;

  constructor(
    private _commonservice: CommonService,
    private _paymentService: PaymentService,
    private fb: FormBuilder,
  ) {
    this.ssForm = fb.group({
      paymentss: fb.control('', [Validators.required]),
      requestId: fb.control(null),
    });
  }

  ngOnInit(): void {
    this.getAllRequest();
    this.dtOptions = {
      pagingType: 'full',
      pageLength: 10
    };
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getAllRequest() {
    this._paymentService.getAllPaymentRequest().subscribe((res) => {
      this.requestData = res;
      this.dtTrigger.next(null);
    });
  }
  upload(id : any){
    this.id = id;
  }

  submit() {
    this.submitted = true;
    if (this.ssForm.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      return;
    }
    const requestId = 1; 

    // Call service to upload the file
    this._paymentService.uploadPaymentVoucher(this.id, this.selectedFile!).subscribe({
      next: (response) => {
        this._commonservice.successAlert("Success", "Request Successfully");
        // Close modal, reset form, or perform additional actions here
      },
      error: (error: HttpErrorResponse) => {
        this._commonservice.successAlert("Success", "Request Successfully");
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  get f() {
    return this.ssForm.controls;
  }
}


