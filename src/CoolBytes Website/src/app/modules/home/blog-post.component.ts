import { ImagesService } from '../../services/imagesservice/images.service';
import { BlogPost } from '../../services/blogpostservice/blog-post';
import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: "./blog-post.component.html",
    styleUrls: ["blog-post.component.css"]
})
export class BlogPostComponent implements OnInit {
    private blogPost: BlogPost;
    private id: number;
    private _authorImage: string;
    
    constructor(private _blogPostsService: BlogPostsService, private _route: ActivatedRoute, private _imagesService : ImagesService) { }

    ngOnInit(): void {
        this.id = parseInt(this._route.snapshot.paramMap.get("id"));
        this._blogPostsService.get(this.id).subscribe(blogPost => this.proccesData(blogPost));
    }

    proccesData(blogPost: BlogPost) {
        this._authorImage = this._imagesService.getUri(blogPost.author.image.uriPath);
        this.blogPost = blogPost;
    }
}