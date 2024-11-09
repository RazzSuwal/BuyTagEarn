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

  userPost2(model: any, file: File): Observable<any> {
    debugger;
    const formData = new FormData();
    formData.append('BrandName', model.brandName || '');
    formData.append('ProductName', model.productName || '');
    formData.append('PostUrl', model.postUrl || '');
    formData.append('PostedOn', model.postedOn || '');
    formData.append('IsTag', model.isTag || '');
    formData.append('file', file);

    const url = `${this.baseUrl}UserPosts`;
    return this.http.post(url, formData, { responseType: 'text' });
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
    return this.http.get(
      `https://localhost:7129/WeatherForecast/ScrapeLikese/${encodedUrl}`,
      {
        responseType: 'json',
      }
    );
  }

  // userPost(model: any, file: File): Observable<any> {
  //   const formData = new FormData();

  //   // Append the properties of the PostRequestDTO to the FormData
  //   formData.append('UserId', model.UserId || '');
  //   formData.append('ProductName', model.ProductName?.toString() || '');
  //   formData.append('PostUrl', model.PostUrl || '');
  //   formData.append('BrandName', model.BrandName || '');
  //   formData.append('IsTag', model.IsTag?.toString() || '');
  //   formData.append('PostedOn', model.PostedOn?.toString() || '');
  //   formData.append('IsPaid', model.IsPaid?.toString() || '');

  //   // Append the file
  //   formData.append('file', file);

  //   // Create the API URL
  //   const url = `${this.baseUrl}UserPosts`; // Assuming this is the correct URL for your endpoint

  //   // Make the HTTP POST request and return the observable
  //   return this.http.post(url, formData);
  // }
}
