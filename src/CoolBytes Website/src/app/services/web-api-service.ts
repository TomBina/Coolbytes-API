import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Headers, RequestOptions, Http } from "@angular/http";
import { Injectable } from '@angular/core';

@Injectable()
export class WebApiService {
    constructor(protected http: Http, protected authService: AuthService) {
    }

    getAuthRequestOptions(headers: Headers): RequestOptions {
        headers = this.authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }

    getRequestOptions(headers: Headers): RequestOptions {
        headers = this.authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }
}