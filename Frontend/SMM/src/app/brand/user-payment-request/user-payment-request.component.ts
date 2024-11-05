import { Component } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { PaymentService } from '../../services/payment/payment.service';

@Component({
  selector: 'app-user-payment-request',
  templateUrl: './user-payment-request.component.html',
  styleUrl: './user-payment-request.component.scss'
})
export class UserPaymentRequestComponent {
  dtOptions: Config={};
  dtTrigger: Subject<any> = new Subject<any>();
  requestData!: any[];

  constructor(
    private _commonservice: CommonService,
    private _paymentService: PaymentService
  ) {}

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

}


