import { ChangeDetectorRef, Component } from '@angular/core';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { PaymentService } from '../../services/payment/payment.service';

@Component({
  selector: 'app-user-earn',
  templateUrl: './user-earn.component.html',
  styleUrl: './user-earn.component.scss'
})
export class UserEarnComponent {
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject<any>();
  datas!: any[];
  constructor(
    private _commonservice: CommonService,
    private cdr: ChangeDetectorRef,
    private _paymentService: PaymentService

  ) {}

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full',
      pageLength: 10,
      processing: true,
      serverSide: false,
      retrieve: true,
      destroy: true,
      language: {
        search: '_INPUT_',
        searchPlaceholder: 'Search records',
      },
      scrollCollapse: false,
    };
    this.getAllData();
  }
  ngAfterViewInit(): void {
    // Trigger DataTable initialization only after the view has been initialized
    // this.dtTrigger.next(null);
  }
  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getAllData() {
    this._paymentService.getAllPaidById().subscribe((res) => {
      this.datas = res;
      this.dtTrigger.next(null);
      this.cdr.detectChanges();
    });
  }
}
