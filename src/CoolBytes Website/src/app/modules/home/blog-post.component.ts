import { ImagesService } from '../../services/imagesservice/images.service';
import { BlogPost } from '../../services/blogpostservice/blog-post';
import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
    templateUrl: "./blog-post.component.html",
    styleUrls: ["blog-post.component.css"]
})
export class BlogPostComponent implements OnInit, OnDestroy {
    private blogPost;
    private _authorImage: string;
    private _onRouteChanges: Subscription;

    constructor(private _blogPostsService: BlogPostsService, private _route: ActivatedRoute, private _imagesService : ImagesService) { }

    ngOnInit(): void {
        this._onRouteChanges = this._route.params.subscribe(changes => this._blogPostsService.get(changes.id).subscribe(blogPost => this.proccesData(blogPost)));

        let id = parseInt(this._route.snapshot.paramMap.get("id"));
        this._blogPostsService.get(id).subscribe(blogPost => this.proccesData(blogPost));
    }

    ngOnDestroy(): void {
        this._onRouteChanges.unsubscribe();
    }

    proccesData(blogPost) {
        if (blogPost.relatedLinks) {
            for (let link of blogPost.relatedLinks) {
                link.url = `/post/${link.id}/${link.subjectUrl}`;
            }
        }

        this._authorImage = this._imagesService.getUri(blogPost.author.image.uriPath);


        this.blogPost = blogPost;
    }
}