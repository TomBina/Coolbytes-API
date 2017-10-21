import { BlogPostUpdateCommand } from './blog-post-update-command';
import { BlogPostAddCommand } from './blog-post-add-command';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { BlogPost } from './blog-post';
import { BlogPostSummary } from './blog-post-summary';
import { BlogPostUpdate } from './blog-post-update';
import { WebApiService } from './../web-api-service';

@Injectable()
export class BlogPostsService extends WebApiService {
    private _url: string = environment.apiUri + "api/blogposts";

    get(blogPostId: number): Observable<BlogPost> {
        let observable = this.http.get(`${this._url}/${blogPostId}`);
        return observable.map((response: Response) => <BlogPost>response.json());
    }

    getAll(): Observable<BlogPostSummary[]> {
        let observable = this.http.get(this._url);
        return observable.map((response: Response) => <BlogPostSummary[]>response.json());
    }

    add(blogPostAdd: BlogPostAddCommand, files: FileList): Observable<BlogPostSummary> {
        let formData = this.createFormData(blogPostAdd, files);
        let observable = this.http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostSummary>response.json());
    }

    getUpdate(blogPostId: number) {
        let observable = this.http.get(`${this._url}/update/${blogPostId}`, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostUpdate>response.json());
    }

    update(blogPostUpdateCommand: BlogPostUpdateCommand, files: FileList): Observable<BlogPostSummary> {
        let formData = this.createFormData(blogPostUpdateCommand, files);
        formData.append("id", blogPostUpdateCommand.id.toString());

        let observable = this.http.put(`${this._url}/update/`, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostSummary>response.json());
    }

    delete(blogPostId: number): Observable<Response> {
        return this.http.delete(`${this._url}/${blogPostId}`, this.getAuthRequestOptions(new Headers()));
    }

    private createFormData(model, files: FileList): FormData {
        let formData = new FormData();
        let file = files && files.length > 0 ? files[0] : null;

        formData.append("subject", model.subject);
        formData.append("contentIntro", model.contentIntro);
        formData.append("content", model.content);

        if (model.tags)
            model.tags.forEach(t => formData.append("tags", t));

        if (model.externalLinks)
            formData.append("externalLinks", JSON.stringify(model.externalLinks));

        if (file)
            formData.append("file", file, file.name);

        return formData;
    }
}
