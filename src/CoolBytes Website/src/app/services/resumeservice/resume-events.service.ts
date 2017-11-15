import { UpdateResumeEventCommand } from './update-resume-event-command';
import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { WebApiService } from '../web-api-service';
import { AddResumeEventCommand } from './add-resume-event-command';
import { ResumeEvent } from './resume-event';

@Injectable()
export class ResumeEventsService extends WebApiService {
    private _url: string = environment.apiUri + "api/resumeevents";
    
    getAll(authorId: number) : Observable<ResumeEvent[]> {
        let observable = this.http.get(`${this._url}/${authorId}/`, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <ResumeEvent[]>response.json());
    }
    
    get(resumeEventId: number) : Observable<ResumeEvent> {
        let observable = this.http.get(`${this._url}/event/${resumeEventId}/`, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <ResumeEvent>response.json());
    }
    
    add(addResumeEventCommand : AddResumeEventCommand) : Observable<ResumeEvent> {
        let observable = this.http.post(this._url, addResumeEventCommand, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <ResumeEvent>response.json());
    }

    update(updateResumeEventCommand : UpdateResumeEventCommand) : Observable<ResumeEvent> {
        let observable = this.http.put(this._url, updateResumeEventCommand, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <ResumeEvent>response.json());
    }

    delete(resumeEventId: number) : Observable<Response> {
        return this.http.delete(`${this._url}/${resumeEventId}/`, this.getAuthRequestOptions(new Headers()));
    }
}