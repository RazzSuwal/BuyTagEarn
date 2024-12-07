import { Component } from '@angular/core';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { BrandService } from '../../services/brand/brand.service';
import { AuthserviceService } from '../../services/auth/authservice.service';

@Component({
  selector: 'app-brand-details',
  templateUrl: './brand-details.component.html',
  styleUrl: './brand-details.component.scss'
})
export class BrandDetailsComponent {
  dtOptions: Config = {};
  dtTrigger: Subject<any> = new Subject<any>();
  requestData!: any[];
  submitted: boolean = false;
  brandForm: UntypedFormGroup;
  formtitle !:string;
  submitBtn !:string;
  selectedProduct: any;
  role = 'BRAND'

  constructor(
    private _commonservice: CommonService,
    private fb: FormBuilder,
    private _brandService: BrandService,
    private _authService: AuthserviceService
  ) {
    this.brandForm = fb.group({
      email: fb.control('', [Validators.required]),
      name: fb.control('', [Validators.required]),
      password: fb.control('', [Validators.required]),
      phoneNumber: fb.control('', [Validators.required]),
      role: fb.control("BRAND"),
    });
  }

  ngOnInit(): void {
    this.getAllBrand();
    this.dtOptions = {
      pagingType: 'full',
      pageLength: 10,
      searching: true,
      lengthChange: true,
      processing: true,
      serverSide: false,
      columnDefs: [
        {
          targets: [3],
          type: 'string',
        }
      ]
    };
    // this.dtTrigger.next(null);
  }
  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getAllBrand() {
    this._brandService.getAllBrand().subscribe((res) => {
      this.requestData = res.data;

      this.dtTrigger.next(null);
    });
  }

  submit() {
    this.submitted = true;
    if (this.brandForm.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      return;
    }
    let data = {
      email: this.brandForm.get('email')?.value,
      name: this.brandForm.get('name')?.value,
      password:  this.brandForm.get('password')?.value,
      phoneNumber:  this.brandForm.get('phoneNumber')?.value,
      role:  this.role
    };
    this._authService.register(data).subscribe({
      next: (res) => {
        this._commonservice.successAlert("Success", "Brand Registered");
        this.brandForm.reset();
        this.submitted = false;

        //need to work on
        // $('#exampleModal').modal('hide');
      },
    });


  }

  get f() {
    return this.brandForm.controls;
  }


  add(){
    this.formtitle = `Add Brand`;
    this.submitBtn = 'Add';
  }

}
