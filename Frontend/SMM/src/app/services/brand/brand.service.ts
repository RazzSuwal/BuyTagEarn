import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BrandService {
  baseUrl: string = 'https://localhost:7091/api/Brand/';

  constructor(private http: HttpClient) {}

  getAllProductById(userId: any): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllProductById/' + userId);
  }

  CreateUpdateProduct(data: any, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('ProductType', data.productType || '');
    formData.append('ProductName', data.productName || '');
    formData.append('ProductId', data.productId || '');
    formData.append('file', file);
    return this.http.post(this.baseUrl + 'CreateUpdateProduct', formData, {
      responseType: 'text',
    });
  }

  getAllBrand(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllBrand');
  }

  approved(productId: number, IsApproved: number): Observable<any> {
    return this.http.put(this.baseUrl + 'AprovedBrandProduct/' + productId + '/' + IsApproved, {}, { responseType: 'text' });
  }
  deleteProductById(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}DeleteProductById/${id}`, {
      responseType: 'text',
    });
  }
  

}
