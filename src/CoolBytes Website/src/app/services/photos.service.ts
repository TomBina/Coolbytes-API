import { Photo } from "./photo";
import { AuthService } from "./auth.service";
import { Injectable } from "@angular/core";
import { Http, RequestOptions, Headers, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

@Injectable()
export class PhotosService {
    private _url: string = "http://localhost:5000/api/photos";

    constructor(private _http: Http, private _authService: AuthService) { }

    getAuthRequestOptions(headers: Headers): RequestOptions {
        headers = this._authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }

    getAll() : Observable<Photo[]> {
        let observable = this._http.get(this._url);
        return observable.map((response: Response) => <Photo[]>response.json());
    }
    
    uploadPhotos(files: FileList): Observable<Photo> {
        let formData = new FormData();

        for (let i = 0; i < files.length; i++)
            formData.append("Files", files[i], files[i].name);

        let observable = this._http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <Photo>response.json());
    }
}