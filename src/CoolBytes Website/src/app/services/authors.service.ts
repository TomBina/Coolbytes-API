import { AuthorAddUpdate } from './author-add-update';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { AuthService } from './auth.service';
import { Author } from './author';

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

    add(author: AuthorAddUpdate): Observable<Author> {
        let formData = this.createFormData(author);
        let observable = this._http.post(this._url, formData, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    update(author: AuthorAddUpdate): Observable<Author> {
        let formData = this.createFormData(author);
        let observable = this._http.put(this._url, formData, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    createFormData(model): FormData {
        let formData = new FormData();
        let file = model.files && model.files.length > 0 ? model.files[0] : null;
        
        formData.append("firstName", model.firstName);
        formData.append("lastName", model.lastName);
        formData.append("about", model.about);

        if (file)
            formData.append("file", file, file.name);

        return formData;
    }
}