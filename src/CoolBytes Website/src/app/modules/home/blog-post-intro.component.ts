import { Component, Input } from "@angular/core";
import { BlogPost } from "../../services/blog-post";

@Component({
    selector: "home-blog-post-intro",
    templateUrl: "./blog-post-intro.component.html",
    styleUrls: ["blog-post-intro.component.css"]
})
export class BlogPostIntroComponent { 
    @Input()
    blogPost: BlogPost;
}