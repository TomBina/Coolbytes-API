import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { Author } from './author';
import { WebApiService } from './../web-api-service';
import { AddOrUpdateAuthorCommand } from './add-or-update-author-command';

@Injectable()
export class AuthorsService extends WebApiService {
    private _url: string = environment.apiUri + "api/authors/";

    get(): Observable<Author> {
        let observable = this.http.get(this._url, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    add(addAuthorCommand: AddOrUpdateAuthorCommand): Observable<Author> {
        let formData = this.createFormData(addAuthorCommand);
        let observable = this.http.post(this._url, formData, this.getRequestOptions(new Headers()));

        return observable.map((response: Response) => <Author>response.json());
    }

    update(updateAuthorCommand: AddOrUpdateAuthorCommand): Observable<Author> {
        let formData = this.createFormData(updateAuthorCommand);
        let observable = this.http.put(this._url, formData, this.getRequestOptions(new Headers()));

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