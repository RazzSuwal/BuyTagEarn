import { Component } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { CommonService } from '../../services/common/common.service';
import { BrandService } from '../../services/brand/brand.service';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthserviceService } from '../../services/auth/authservice.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss',
})
export class ProductComponent {
  dtOptions: Config = {};
  dtTrigger: Subject<any> = new Subject<any>();
  requestData!: any[];
  submitted: boolean = false;
  productForm: UntypedFormGroup;
  formtitle !:string;
  submitBtn !:string;
  selectedProduct: any;
  userId: string | null = null;

  constructor(
    private _commonservice: CommonService,
    private _brandService: BrandService,
    private fb: FormBuilder,
    private authService: AuthserviceService
  ) {
    this.productForm = fb.group({
      productName: fb.control('', [Validators.required]),
      productType: fb.control('', [Validators.required]),
      productId: fb.control(null),
    });
  }

  ngOnInit(): void {
    this.getUserID();
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
    this.dtTrigger.next(null);
  }
  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }
  getUserID(): void {
    debugger
    this.userId = this.authService.getUserID();
    if (this.userId != null) {
      this.getAllProductById(this.userId);
    }

  }
  getAllProductById(userId: any): void {
    this._brandService.getAllProductById(userId).subscribe((res) => {
      this.requestData = res.data;
      console.log('Request Data:', this.requestData);
      // Destroy the DataTable instance if already initialized
      if ($.fn.dataTable.isDataTable('table')) {
        $('table').DataTable().destroy();
      }
  
      // Trigger DataTable reinitialization after ensuring data is available
      setTimeout(() => {
        this.dtTrigger.next(null); // Passing null or any value to avoid TypeScript errors
      });
    });
    console.log(this.requestData);
    
  }

  submit() {
    this.submitted = true;
    if (this.productForm.invalid) {
      this._commonservice.warning('Error', 'Please fill up required fields!');
      return;
    }
    let data = {
      productName: this.productForm.get('productName')?.value,
      productType: this.productForm.get('productType')?.value,
      productId:  this.productForm.get('productId')?.value
    };
    const fileInput = (document.getElementById('file') as HTMLInputElement).files;
    if (!fileInput || fileInput.length === 0) {
      this._commonservice.warning('Error', 'Please upload the file!');
      return;
    }

    const file = fileInput[0];
    this._brandService.CreateUpdateProduct(data, file).subscribe({
      next: (res) => {
        this._commonservice.successAlert("Success", res);
        this.productForm.reset();
        this.submitted = false;
        this.getAllProductById(this.userId );
        //need to work on
        // $('#exampleModal').modal('hide');
      },
    });
   

  }

  get f() {
    return this.productForm.controls;
  }

  btnedit(product: any) {
    this.formtitle = `Update Product`;
    this.submitBtn = 'Update';

    this.selectedProduct = product;

    this.productForm.patchValue({
      productName: product.ProductName,
      productType: product.ProductType,
      productId: product.ProductId
    });
  }

  add(){
    this.formtitle = `Add Product`;
    this.submitBtn = 'Add';
  }

  delete(id: any)
  {
    this._brandService.deleteProductById(id).subscribe({
      next: (res) => {
        this._commonservice.successAlert("Success", res);

        //need to work on
        // $('#exampleModal').modal('hide');
      },
    });
    this.getAllProductById(this.userId );
    
  }

}
