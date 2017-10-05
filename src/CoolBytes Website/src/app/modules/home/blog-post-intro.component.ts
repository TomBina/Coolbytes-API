import { Component, Input, Output, EventEmitter } from "@angular/core";
import { BlogPost } from "../../services/blog-post";

@Component({
    selector: "home-blog-post-intro",
    templateUrl: "./blog-post-intro.component.html",
    styleUrls: ["blog-post-intro.component.css"]
})
export class BlogPostIntroComponent { 
    @Input()
    blogPost: BlogPost;

    @Input()
    cssClass: string;

    @Output()
    onBlogPostMouseEnter = new EventEmitter<BlogPost>();
    
    @Output()
    onBlogPostMouseLeave = new EventEmitter<BlogPost>();

    onBlogPostMouseEnterHandler() {
        this.onBlogPostMouseEnter.emit(this.blogPost);
    }

    onBlogPostMouseLeaveHandler() {
        this.onBlogPostMouseLeave.emit(this.blogPost);
    }
}