import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl: string = 'https://localhost:7091/api/LocalUser/';

  constructor(private http: HttpClient) {}
  userPost(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'UserPosts', data, {
      responseType: 'text',
    });
  }
  getUserPost(userId: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserPosts/' + userId);
  }
  getUserPostsDetails(userPostId: number): Observable<any> {
    return this.http.get(this.baseUrl + 'GetUserPostsDetails/' + userPostId);
  }
  getEmbedLikesAndUrl(postUrl: string): Observable<any> {
    // Encode the URL to handle special characters
    const encodedUrl = encodeURIComponent(postUrl);
    return this.http.get(`https://localhost:7129/WeatherForecast/ScrapeLikese/${encodedUrl}`, {
        responseType: 'json',
    });
}

}
