import { Injectable } from '@angular/core';
import { AngularFireStorage } from '@angular/fire/storage';
import { ApiService } from './api.service';

import {v4 as uuidv4} from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  constructor(private fireStorage : AngularFireStorage, private apiService: ApiService) { }

  processFile(file: File, apiKey: string) {
    const userId = localStorage.getItem('firebaseUserID');
    const uploadUid = uuidv4();
    this.fireStorage.upload(`${userId}/${uploadUid}`, file)
      .then(snapshot => {
        console.log('File upload completed');
        return this.apiService.equeueUpload(uploadUid, apiKey, file.name).toPromise();
      })
      .then(value => {
        console.log('Upload enqueued');
      });
  }

}
