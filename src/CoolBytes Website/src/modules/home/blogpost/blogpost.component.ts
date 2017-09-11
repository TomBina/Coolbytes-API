import { Component, Input } from "@angular/core";
import { BlogPost } from "../../../services/blogpost";

@Component({
    selector: "home-blogpost",
    templateUrl: "./blogpost.component.html",
    styleUrls: ["blogpost.component.css"]
})
export class BlogPostComponent { 
    
    @Input()
    blogPost: BlogPost;
}