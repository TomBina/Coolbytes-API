import { AuthService } from './auth.service';
import 'rxjs/add/operator/map';

import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { Author } from "./author";

@Injectable()
export class AuthorsService {
    
    constructor(private _http: Http, private _authService: AuthService) { }

    private _url: string = "http://localhost:5000/api/authors/";

    getRequestOptions(headers: Headers): RequestOptions {
        headers = this._authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }

    get(): Observable<Author> {
        let observable = this._http.get(this._url, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    add(author: Author) : Observable<Author> {
        let observable = this._http.post(this._url, author, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }
}
