import { AddBlogPostCommand } from './add-blog-post-command';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../../environments/environment';
import { BlogPost } from './blog-post';
import { BlogPostSummary } from './blog-post-summary';
import { BlogPostUpdate } from './blog-post-update';
import { WebApiService } from './../web-api-service';
import { UpdateBlogPostCommand } from './update-blog-post-command';

@Injectable()
export class BlogPostsService extends WebApiService {
    private _url: string = environment.apiUri + "api/blogposts";

    get(blogPostId: number): Observable<BlogPost> {
        let observable = this.http.get(`${this._url}/${blogPostId}`);
        return observable.map((response: Response) => <BlogPost>response.json());
    }

    getAll(tag?: string): Observable<BlogPostSummary[]> {
        let observable = tag ? this.http.get(`${this._url}/?tag=${encodeURIComponent(tag)}`) : this.http.get(this._url);
        return observable.map((response: Response) => <BlogPostSummary[]>response.json());
    }

    add(addBlogPostCommand: AddBlogPostCommand, files: FileList): Observable<BlogPostSummary> {
        let formData = this.createFormData(addBlogPostCommand, files);
        let observable = this.http.post(this._url, formData, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostSummary>response.json());
    }

    getUpdate(blogPostId: number) {
        let observable = this.http.get(`${this._url}/update/${blogPostId}`, this.getAuthRequestOptions(new Headers()));
        return observable.map((response: Response) => <BlogPostUpdate>response.json());
    }

    update(updateBlogPostCommand: UpdateBlogPostCommand, files: FileList): Observable<BlogPostSummary> {
        let formData = this.createFormData(updateBlogPostCommand, files);
        formData.append("id", updateBlogPostCommand.id.toString());

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
