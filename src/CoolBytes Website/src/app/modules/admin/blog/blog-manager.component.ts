import { BlogPost } from '../../../services/blog-post';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Author } from '../../../services/author';
import { BlogPostsService } from '../../../services/blog-posts.service';

@Component({
    templateUrl: "./blog-manager.component.html",
    styleUrls: ["./blog-manager.component.css"]
})
export class BlogManagerComponent implements OnInit {
    private _blogPosts: BlogPost[];

    constructor(private _blogPostsService: BlogPostsService) {

    }

    ngOnInit(): void {
        this.getBlogs();
    }

    getBlogs(): void {
        this._blogPostsService.getAll().subscribe(blogPosts => this._blogPosts = blogPosts);
    }

    delete(blogPostId: number): void {
        if (!confirm("Are you sure?"))
            return;
            
        this._blogPostsService.delete(blogPostId).subscribe(response => {
            if (response.ok)
                this.getBlogs();
        });
    }
}