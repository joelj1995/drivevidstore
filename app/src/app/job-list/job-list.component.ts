import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { JobService } from '../job.service';
import { QueuedJob } from './queued-job';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.css']
})
export class JobListComponent implements OnInit {

  jobs: Observable<QueuedJob[]>;

  constructor(private jobService: JobService) { }

  ngOnInit(): void {
    this.jobs = this.jobService.queuedJobs;
  }

}
