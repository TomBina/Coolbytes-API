import { Enumerate } from '../../utils/enumerate';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { Author } from './author';
import { WebApiService } from './../web-api-service';

@Injectable()
export class AuthorsService extends WebApiService {
    private _url: string = environment.apiUri + "api/authors/";

    get(): Observable<Author> {
        let observable = this.http.get(this._url, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    getWithProfile(): Observable<Author> {
        let observable = this.http.get(`${this._url}?includeProfile=true`, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    add(command): Observable<Author> {
        let observable = this.http.post(this._url, command, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    update(command): Observable<Author> {
        let observable = this.http.put(this._url, command, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }
}