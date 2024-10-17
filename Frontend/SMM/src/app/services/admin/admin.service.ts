import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl: string = 'https://localhost:7091/api/Admin/';

  constructor(private http: HttpClient) { 

  }
  getAllUserPost(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserPosts/' + null);
  }
  approved(postId: number, IsApproved: number): Observable<any> {
    return this.http.post(this.baseUrl + 'AprovedUserPost/' + postId + '/' + IsApproved, {}, { responseType: 'text' });
  }
  
  
}
