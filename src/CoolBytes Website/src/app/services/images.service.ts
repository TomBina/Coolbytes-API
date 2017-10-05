import { environment } from '../../environments/environment';
import { Image } from "./image";
import { AuthService } from "./auth.service";
import { Injectable } from "@angular/core";
import { Http, RequestOptions, Headers, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

@Injectable()
export class ImagesService {
    private _url: string = environment.apiUri + "api/images";

    constructor(private _http: Http, private _authService: AuthService) { }

    getAuthRequestOptions(headers: Headers): RequestOptions {
        headers = this._authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }

    getAll() : Observable<Image[]> {
        let observable = this._http.get(this._url);
        return observable.map((response: Response) => <Image[]>response.json());
    }
    
    uploadImages(files: FileList): Observable<Image> {
        let formData = new FormData();

        for (let i = 0; i < files.length; i++)
            formData.append("Files", files[i], files[i].name);

        let observable = this._http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <Image>response.json());
    }

    getUri(uriPath: string) {
        if (!uriPath)
            return;

        let length = environment.imagesUri.length;
        return environment.imagesUri.substring(0, --length) + uriPath;
    }
}