import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { Image } from './image';
import { WebApiService } from './../web-api-service';

@Injectable()
export class ImagesService extends WebApiService {
    private _url: string = environment.apiUri + "api/images/";

    getAll(): Observable<Image[]> {
        let observable = this.http.get(this._url, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <Image[]>response.json());
    }

    uploadImages(files: FileList): Observable<Image> {
        let formData = new FormData();

        for (let i = 0; i < files.length; i++)
            formData.append("Files", files[i], files[i].name);

        let observable = this.http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <Image>response.json());
    }

    delete(imageId: number) {
        let observable = this.http.delete(`${this._url}?id=${imageId}`, this.getAuthRequestOptions(new Headers()));
        return observable;
    }

    getUri(uriPath: string) {
        if (!uriPath)
            return;

        let length = environment.imagesUri.length;
        return environment.imagesUri.substring(0, --length) + uriPath;
    }
}