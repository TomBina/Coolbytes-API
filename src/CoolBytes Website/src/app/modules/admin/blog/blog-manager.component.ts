import { BlogPostsService } from '../../../services/blogpostservice/blog-posts.service';
import { BlogPostSummary } from '../../../services/blogpostservice/blog-post-summary';
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";


@Component({
    templateUrl: "./blog-manager.component.html",
    styleUrls: ["./blog-manager.component.css"]
})
export class BlogManagerComponent implements OnInit {
    blogPosts: BlogPostSummary[];

    constructor(private _blogPostsService: BlogPostsService) {

    }

    ngOnInit(): void {
        this.getBlogs();
    }

    getBlogs(): void {
        this._blogPostsService.getAll().subscribe(blogPosts => this.blogPosts = blogPosts);
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