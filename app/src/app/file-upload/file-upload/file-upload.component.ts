import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AngularFireStorage } from '@angular/fire/storage';
import { ApiService } from 'src/app/api.service';

import {v4 as uuidv4} from 'uuid';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  apiKey?: string = null;

  constructor(private fireStorage : AngularFireStorage, private apiService: ApiService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    console.log(this.route.url);
    this.route.fragment.subscribe((fragment: string) => {
      var p = new URLSearchParams(fragment);
      if (p.get('access_token')) {
        this.apiKey = p.get('access_token');
        console.log('api key set');
      }
    })
  }

  getAuthToken() {
    // TODO: inject the client id and redirect
    window.location.href = "https://accounts.google.com/o/oauth2/v2/auth?client_id=968753614509-8pnb23be87158145scn9q1e7aa97pcbu.apps.googleusercontent.com&redirect_uri=http://localhost:4200&response_type=token&scope=https://www.googleapis.com/auth/drive.file";
  }

  uploadFile(files: FileList) {
    const fileToUpload = files.item(0);
    const userId = localStorage.getItem('firebaseUserID');
    const uploadUid = uuidv4();
    const uploadRef = this.fireStorage.ref(`${userId}/${uploadUid}`);
    console.log(`Uploading to ${userId}/${uploadUid} using key ${this.apiKey}`);
    this.fireStorage.upload(`${userId}/${uploadUid}`, fileToUpload)
      .then(snapshot => {
        console.log('File upload completed');
        return this.apiService.equeueUpload(uploadUid, this.apiKey, fileToUpload.name).toPromise();
      })
      .then(value => {
        console.log('Upload enqueued');
      });
  }

}
