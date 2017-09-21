import "rxjs/add/operator/map";

import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { Author } from "./author";

@Injectable()
export class AuthorsService {
    
    constructor(private _http: Http) { }

    private _author: Author;
    private _url: string = "http://localhost:5000/api/authors/";

    getRequestOptions(headers: Headers): RequestOptions {
        headers.append("Authorization", "Bearer " + localStorage.getItem("access_token"));
        return new RequestOptions({ headers: headers });
    }

    get(): Observable<Author> {
        if (this._author)
            return Observable.of(this._author);

        let observable = this._http.get(this._url, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json()).do(author => this._author = author);
    }

    save(author: Author) : Observable<Author> {
        let observable = this._http.post(this._url, author, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json()).do(author => this._author = author);
    }

    clearCache() {
        this._author = undefined;
    }
}
