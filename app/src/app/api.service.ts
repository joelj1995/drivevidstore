import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from './../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  BASE_URL: string = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  createHeaders() {
    var jwt = localStorage.getItem('firebaseJWT');
    return {headers: new HttpHeaders().set('Authorization', `Bearer ${jwt}`)};
  }

  getPath(path: string) {
    return this.http.get(this.BASE_URL + path, this.createHeaders());
  }

  postPath(path: string, body: any) {
    return this.http.post(this.BASE_URL + path, body, this.createHeaders());
  }

  getDriveApiKeys() {
    return this.getPath('/profile/api-keys');
  }

  equeueUpload(identifier: string, apiKey: string, fileName: string) {
    return this.postPath(`/job/${identifier}`, { 'ApiKey' : apiKey, 'FileName': fileName});
  }
}
