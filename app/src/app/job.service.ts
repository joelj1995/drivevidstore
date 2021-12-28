import { Injectable } from '@angular/core';
import { AngularFireStorage } from '@angular/fire/storage';
import { ApiService } from './api.service';

import {v4 as uuidv4} from 'uuid';
import { QueuedJob } from './job-list/queued-job';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  private _queue: QueuedJob[];
  private _queueSubject: BehaviorSubject<QueuedJob[]>;

  constructor(private fireStorage : AngularFireStorage, private apiService: ApiService) { 
    this._queue = [];
    this._queueSubject = new BehaviorSubject<QueuedJob[]>([]);
  }

  get queuedJobs(): Observable<QueuedJob[]> {
    return this._queueSubject.asObservable();
  }

  private addToQueue(job: QueuedJob) {
    this._queue.push(job);
    this._queueSubject.next(this._queue);
  }

  private transitionStatus(jobId: string, status: string) {
    const idx = this._queue.findIndex(j => j.id == jobId);
    if (idx >= 0) {
      this._queue[idx].status = status;
      this._queueSubject.next(this._queue);
    }
  }

  private popFromQueue(jobId: string) {
    this._queue = this._queue.filter(i => i.id != jobId);
    this._queueSubject.next(this._queue);
  }

  processFile(file: File, apiKey: string) {
    const userId = localStorage.getItem('firebaseUserID');
    const uploadUid = uuidv4();
    let jobToQueue = new QueuedJob();
    jobToQueue.id = uploadUid;
    jobToQueue.fileName = file.name;
    jobToQueue.status = 'Uploading';
    jobToQueue.started = new Date();
    this.addToQueue(jobToQueue);
    this.fireStorage.upload(`${userId}/${uploadUid}`, file)
      .then(snapshot => {
        console.log('File upload completed');
        this.transitionStatus(uploadUid, 'Uploading');
        return this.apiService.equeueUpload(uploadUid, apiKey, file.name).toPromise();
      })
      .then(value => {
        console.log('Upload enqueued');
        this.transitionStatus(uploadUid, 'Enqueued');
        // this.popFromQueue(uploadUid); // enable this when tracking of server side jobs supported
      });
  }
}
