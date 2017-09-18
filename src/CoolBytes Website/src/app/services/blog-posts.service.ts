import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Injectable } from "@angular/core"
import { Observable } from "rxjs/Observable";
import { BlogPost } from "./blog-post";

import "rxjs/add/operator/map";

@Injectable()
export class BlogPostsService {
    constructor(private _http: Http) {

    }

    getAll() : Observable<BlogPost[]> {
        let headers = new Headers();
        headers.append("Authorization","Bearer "+localStorage.getItem("access_token"));
        let options = new RequestOptions({headers: headers});

        let observable = this._http.get("http://localhost:5000/api/blogposts/", options);
        return observable.map((response: Response) => <BlogPost[]>response.json());
    }
}
