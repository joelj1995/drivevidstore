import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

  sub!: Subscription;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.sub = this.apiService.getDriveApiKeys().subscribe(s => {
      console.log(s);
    });
  }

}
