import { BlogPost } from '../../services/blog-post';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BlogPostsService } from '../../services/blog-posts.service';

@Component({
    templateUrl: "./blog-post.component.html",
    styleUrls: ["blog-post.component.css"]
})
export class BlogPostComponent implements OnInit {
    private blogPost: BlogPost;
    private id: number;

    constructor(private _blogPostsService: BlogPostsService, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this.id = parseInt(this._route.snapshot.paramMap.get("id"));
        this._blogPostsService.get(this.id).subscribe(blogPost => this.blogPost = blogPost);
    }
}