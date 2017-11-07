import { ImagesService } from '../../services/imagesservice/images.service';
import { BlogPostSummary } from '../../services/blogpostservice/blog-post-summary';
import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";

@Component({
    selector: "home-blog-post-intro",
    templateUrl: "./blog-post-intro.component.html",
    styleUrls: ["blog-post-intro.component.css"]
})
export class BlogPostIntroComponent { 
    @Input()
    set blogPost(value: BlogPostSummary) {
        this._blogPost = value;

        if (value.image)
            this._imageUrl = this._imagesService.getUri(value.image.uriPath);
    };

    private _blogPost: BlogPostSummary;
    private _imageUrl: string;

    constructor(private _imagesService: ImagesService) {
    }
}