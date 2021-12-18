import { Component, OnInit } from '@angular/core';
import { AngularFireStorage } from '@angular/fire/storage';
import { ApiService } from 'src/app/api.service';

import {v4 as uuidv4} from 'uuid';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  constructor(private fireStorage : AngularFireStorage, private apiService: ApiService) { }

  ngOnInit(): void {
  }

  uploadFile(files: FileList) {
    const fileToUpload = files.item(0);
    const userId = localStorage.getItem('firebaseUserID');
    const uploadUid = uuidv4();
    const uploadRef = this.fireStorage.ref(`${userId}/${uploadUid}`);
    console.log(`Uploading to ${userId}/${uploadUid}`);
    this.fireStorage.upload(`${userId}/${uploadUid}`, fileToUpload)
      .then(snapshot => {
        console.log('File upload completed');
        return this.apiService.equeueUpload(uploadUid, 'foo').toPromise();
      })
      .then(value => {
        console.log('Upload enqueued');
      });
  }

}
