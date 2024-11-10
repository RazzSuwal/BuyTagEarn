import { Component } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { BrandService } from '../../services/brand/brand.service';

@Component({
  selector: 'app-brandproducts',
  templateUrl: './brandproducts.component.html',
  styleUrl: './brandproducts.component.scss'
})
export class BrandproductsComponent {
  dtOptions: Config = {};
  dtTrigger: Subject<any> = new Subject<any>();
  products!: any[];

  constructor(
    private _commonservice: CommonService,
    private _brand: BrandService,
  ) {}

  ngOnInit(): void {
    this.getBrandProducts();
    this.dtOptions = {
      pagingType: 'full',
      // pageLength: 10
    };
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getBrandProducts() {
    this._brand.getAllProductById(-1).subscribe((res) => {
      this.products = res.data;
      this.dtTrigger.next(null);
    });
  }
  approved(productId: number, IsApproved: number) {
    this._brand.approved(productId, IsApproved).subscribe(
      (res) => {
        if (IsApproved == 1) {
          this._commonservice.successAlert('Success', 'Approved SucessFully');
        } else {
          this._commonservice.successAlert('Success', 'Cancel SucessFully');
        }
      },
      (err) => {
        this._commonservice.error('Error', err);
      }
    );
  }
}
