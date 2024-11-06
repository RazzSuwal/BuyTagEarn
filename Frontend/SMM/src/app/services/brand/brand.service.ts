import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  baseUrl: string = 'https://localhost:7091/api/Brand/';

  constructor(private http: HttpClient) {

  }

  getAllProductById(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllProductById' );
  }

  CreateUpdateProduct(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'CreateUpdateProduct', data, {
      responseType: 'text',
    });
  }
}
