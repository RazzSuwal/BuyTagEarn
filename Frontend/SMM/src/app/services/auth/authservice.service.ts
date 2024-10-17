import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthserviceService {
  baseUrl: string = 'https://localhost:7091/api/Users/';
  userStatus: Subject<string> = new Subject();

  constructor(private http: HttpClient) {}

  register(user: any): Observable<any> {
    return this.http.post(this.baseUrl + 'register', user, {
      responseType: 'text',
    });
  }

  login(user: any): Observable<any> {
    return this.http.post(this.baseUrl + 'login', user);
  }

  getUserRole(): Observable<string | null> {
    const token = localStorage.getItem('authToken');
  
    if (token) {
      const userDetails = JSON.parse(atob(token.split('.')[1]));
      const email = userDetails.email;
  
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`  // Include JWT token in headers
      });
  
      return this.http.get(`${this.baseUrl}GetUserRole/${email}`, { headers, responseType: 'text' });
    }
  
    return of(null);
  }
  

  getUserID(): string | null {
    const token = localStorage.getItem('authToken');
    if (token) {
      const userDetails = JSON.parse(atob(token.split('.')[1]));
      return userDetails.sub;
    }
    return null;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('authToken');
  }

  logout() {
    localStorage.removeItem('authToken');
  }
}
