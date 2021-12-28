import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../api.service';
import { JobService } from '../job.service';

const apiKeyTimeout: number = 1000 * 60 * 15;

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.css']
})
export class MainContentComponent implements OnInit {

  apiKey?: string = null;
  authValidated: boolean = false;

  constructor(private route: ActivatedRoute, private jobService: JobService, private apiService: ApiService) {
    this.route.fragment.subscribe((fragment: string) => {
      var p = new URLSearchParams(fragment);
      if (p.get('access_token')) {
        this.apiKey = p.get('access_token');
        setTimeout(() =>{
          this.apiKey = null;
        }, apiKeyTimeout)
      }
    });
   }

  ngOnInit(): void {
    this.pingApi().then(() => {
      this.authValidated = true;
    });
  }

  pingApi() {
    return this.apiService.getDriveApiKeys().toPromise();
  }

  clearToken() {
    this.apiKey = null;
  }

  uploadFileChanged(files: FileList) {
    this.pingApi().then(() => {
      const fileToUpload = files.item(0);
      if (fileToUpload) { // didn't cancel the operation
        this.jobService.processFile(fileToUpload, this.apiKey);
      }
    });
  }

  getAuthToken() {
    // TODO: inject the client id and redirect
    window.location.href = "https://accounts.google.com/o/oauth2/v2/auth?client_id=968753614509-8pnb23be87158145scn9q1e7aa97pcbu.apps.googleusercontent.com&redirect_uri=http://localhost:4200&response_type=token&scope=https://www.googleapis.com/auth/drive.file";
  }

}
