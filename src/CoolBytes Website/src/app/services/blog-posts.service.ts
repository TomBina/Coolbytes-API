import { BlogPostUpdate } from './blog-post-update';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { AuthService } from './auth.service';
import { BlogPost } from './blog-post';
import { BlogPostAdd } from './blog-post-add';

@Injectable()
export class BlogPostsService {
    constructor(private _http: Http, private _authService: AuthService) { }
    private _url: string = "http://localhost:5000/api/blogposts";

    getAuthRequestOptions(headers: Headers): RequestOptions {
        headers = this._authService.addAuthorizationHeader(headers);
        return new RequestOptions({ headers: headers });
    }

    get(blogPostId: number): Observable<BlogPost> {
        let observable = this._http.get(`${this._url}/${blogPostId}`);
        return observable.map((response: Response) => <BlogPost>response.json());
    }

    getAll(): Observable<BlogPost[]> {
        let observable = this._http.get(this._url);
        return observable.map((response: Response) => <BlogPost[]>response.json());
    }

    add(blogPostAdd: BlogPostAdd): Observable<BlogPostAdd> {
        let observable = this._http.post(this._url, blogPostAdd, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostAdd>response.json());
    }

    update(blogPostUpdate: BlogPostUpdate): Observable<BlogPostAdd> {
        let observable = this._http.put(this._url, blogPostUpdate, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostUpdate>response.json());
    }

    delete(blogPostId: number): Observable<Response> {
        return this._http.delete(`${this._url}/${blogPostId}`, this.getAuthRequestOptions(new Headers()));
    }
}
