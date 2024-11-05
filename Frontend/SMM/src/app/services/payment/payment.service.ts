import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  baseUrl: string = 'https://localhost:7091/api/Payment/';
  constructor(private http: HttpClient) { }

  paymentRequest(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'PaymentRequest', data, {
      responseType: 'text',
    });
  }

  getAllPaymentRequest(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllPaymentRequest' );
  }
}
