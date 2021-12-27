import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.css']
})
export class MainContentComponent implements OnInit {

  apiKey?: string = null;

  constructor(private route: ActivatedRoute) {
    this.route.fragment.subscribe((fragment: string) => {
      var p = new URLSearchParams(fragment);
      if (p.get('access_token')) {
        this.apiKey = p.get('access_token');
      }
    });
   }

  ngOnInit(): void {
  }

  runUpload() {

  }

  clearToken() {
    this.apiKey = null;
  }

  getAuthToken() {
    // TODO: inject the client id and redirect
    window.location.href = "https://accounts.google.com/o/oauth2/v2/auth?client_id=968753614509-8pnb23be87158145scn9q1e7aa97pcbu.apps.googleusercontent.com&redirect_uri=http://localhost:4200&response_type=token&scope=https://www.googleapis.com/auth/drive.file";
  }

}
