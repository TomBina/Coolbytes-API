import { environment } from '../../environments/environment';
import 'rxjs/add/operator/map';

import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { AuthService } from "./auth.service";
import { BlogPost } from "./blog-post";
import { BlogPostAdd } from "./blog-post-add";
import { BlogPostUpdate } from "./blog-post-update";

@Injectable()
export class BlogPostsService {
    constructor(private _http: Http, private _authService: AuthService) { }
    private _url: string = environment.apiUri + "api/blogposts";

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

    add(blogPostAdd: BlogPostAdd, files: FileList): Observable<BlogPostAdd> {
        let formData = this.createFormData(blogPostAdd, files);
        let observable = this._http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostAdd>response.json());
    }

    update(blogPostUpdate: BlogPostUpdate, files: FileList): Observable<BlogPostUpdate> {
        let formData = this.createFormData(blogPostUpdate, files);
        formData.append("id", blogPostUpdate.id.toString());

        let observable = this._http.put(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostUpdate>response.json());
    }

    private createFormData(model, files: FileList): FormData {
        let formData = new FormData();
        let file = files && files.length > 0 ? files[0] : null;

        formData.append("subject", model.subject);
        formData.append("contentIntro", model.contentIntro);
        formData.append("content", model.content);

        if (model.tags)
            model.tags.forEach(t => {
                formData.append("tags", t);
            });

        if (file)
            formData.append("file", file, file.name);

        return formData;
    }

    delete(blogPostId: number): Observable<Response> {
        return this._http.delete(`${this._url}/${blogPostId}`, this.getAuthRequestOptions(new Headers()));
    }
}
