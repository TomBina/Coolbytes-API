import { environment } from '../../../environments/environment';
import { ImagesService } from '../../services/imagesservice/images.service';
import { BlogPost } from '../../services/blogpostservice/blog-post';
import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
    templateUrl: "./blog-post.component.html",
    styleUrls: ["blog-post.component.css"]
})
export class BlogPostComponent implements OnInit, OnDestroy {
    blogPost;
    authorImage: string;
    shareInfo;

    private _onRouteChanges: Subscription;
    
    constructor(private _blogPostsService: BlogPostsService, private _route: ActivatedRoute, private _imagesService : ImagesService, private _router: Router, private _titleService: Title) { }

    ngOnInit(): void {
        this._onRouteChanges = this._route.params.subscribe(changes => this._blogPostsService.get(changes.id).subscribe(blogPost => this.proccesData(blogPost)));
    }

    ngOnDestroy(): void {
        this._onRouteChanges.unsubscribe();
    }

    proccesData(blogPost) {
        this._titleService.setTitle(`${blogPost.subject} - Cool Bytes`);
        window.scrollTo(0,0);
        
        this.shareInfo = { 
            url: `${environment.appUri}post/${blogPost.id}/${blogPost.subjectUrl}`,
            subject: blogPost.subject
        };
        
        if (blogPost.relatedLinks) {
            for (let link of blogPost.relatedLinks) {
                link.url = `/post/${link.id}/${link.subjectUrl}`;
            }
        }

        this.authorImage = this._imagesService.getUri(blogPost.author.image.uriPath);


        this.blogPost = blogPost;
    }
}