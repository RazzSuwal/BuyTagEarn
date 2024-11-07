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

  uploadPaymentVoucher(requestId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    const url = `${this.baseUrl}UploadVoucher?requestId=${requestId}`;

    return this.http.put(url, formData);
  }
}
