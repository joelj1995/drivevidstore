import { Component } from '@angular/core';
import { JobService } from './job.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'drive-vid-store-ui';

  activeJobsCount: number = 0;

  constructor (private jobService: JobService) {

  }

  ngOnInit() {
    this.jobService.queuedJobs.subscribe(jobs => {
      this.activeJobsCount = jobs.length;
    })
  }

}
