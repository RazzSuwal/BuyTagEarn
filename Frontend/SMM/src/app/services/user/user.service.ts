import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl: string = 'https://localhost:7091/api/LocalUser/';

  constructor(private http: HttpClient) { 

  }
  userPost(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'UserPosts', data, {
      responseType: 'text',
    });
  }
  getUserPost(userId: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserPosts/' + userId);
  }
 
}
