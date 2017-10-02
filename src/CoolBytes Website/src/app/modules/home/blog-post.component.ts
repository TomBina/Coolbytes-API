import { Component, Input } from "@angular/core";
import { BlogPost } from "../../services/blog-post";

@Component({
    templateUrl: "./blog-post.component.html",
    styleUrls: ["blog-post.component.css"]
})
export class BlogPostComponent { 
    @Input()
    blogPost: BlogPost;
}