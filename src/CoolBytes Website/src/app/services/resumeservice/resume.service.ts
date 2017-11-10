import { Resume } from './resume';
import { WebApiService } from '../web-api-service';
import { environment } from '../../../environments/environment';
import { Headers, Response } from '@angular/http';
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ResumeService extends WebApiService {
    private _url: string = environment.apiUri + "api/resume";

    get(authorId: number) : Observable<Resume> {
        let observable = this.http.get(`${this._url}/${authorId}/`, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <Resume>response.json());
    }
}