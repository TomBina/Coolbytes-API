import { BlogPostSummary } from '../../services/blog-post-summary';
import { Component, OnInit } from "@angular/core";
import { BlogPostsService } from "../../services/blog-posts.service";
import { BlogPost } from "../../services/blog-post";

@Component({
    templateUrl: "./blog.component.html",
    styleUrls: ["./blog.component.css"]
})
export class BlogComponent implements OnInit {
    private _blogPosts: BlogPostViewModel[];

    constructor(private _blogpostsService: BlogPostsService) {

    }

    ngOnInit(): void {
        this._blogpostsService.getAll().map(blogPosts => {
            let blogPostsViewModel: BlogPostViewModel[] = [];

            blogPosts.forEach(blogPost => {
                let blogPostViewModel = new BlogPostViewModel();
                blogPostViewModel.blogPost = blogPost;
                blogPostsViewModel.push(blogPostViewModel);
            });

            return blogPostsViewModel;
        }).subscribe(blogPosts => { this._blogPosts = blogPosts; });
    }

    onBlogPostMouseEnterHandler(blogPost: BlogPost) {
        this._blogPosts.forEach(b => { 
            if (b.blogPost.id != blogPost.id)
                b.cssClass = "post tobackground";
        });
    }

    onBlogPostMouseLeaveHandler(blogPost: BlogPost) {
        this._blogPosts.forEach(b => { 
            if (b.blogPost.id != blogPost.id)
                b.cssClass = "post";
        });
    }
}

export class BlogPostViewModel {
    blogPost: BlogPostSummary;
    cssClass: string = "post";
}