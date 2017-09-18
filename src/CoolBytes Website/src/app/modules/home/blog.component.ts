import { Component, OnInit } from "@angular/core";
import { BlogPostsService } from "../../services/blog-posts.service";
import { BlogPost } from "../../services/blog-post";

@Component({
    templateUrl: "./blog.component.html",
    styleUrls: ["./blog.component.css"]
})
export class BlogComponent implements OnInit {
    blogPosts: BlogPost[];

    constructor(private _blogpostsService: BlogPostsService) {
        
    }

    ngOnInit(): void {
        this._blogpostsService.getAll().subscribe(blogPosts => {
            this.blogPosts = blogPosts;
        })
    }

}