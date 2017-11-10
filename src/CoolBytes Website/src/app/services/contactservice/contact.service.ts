import { environment } from '../../../environments/environment';
import { WebApiService } from '../web-api-service';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ContactService extends WebApiService {
    private _url: string = environment.apiUri + "api/contact";
    
    public Send(command) : Observable<Response> {
        let observable = this.http.post(this._url, command);
        return observable;
    }
}