import { BlogPostSummary } from '../../services/blogpostservice/blog-post-summary';
import { BlogPost } from '../../services/blogpostservice/blog-post';
import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: "./blog.component.html",
    styleUrls: ["./blog.component.css"]
})
export class BlogComponent implements OnInit {
    private _blogPosts: BlogPostViewModel[];
    private _tag: string;

    constructor(private _blogpostsService: BlogPostsService, private _route: ActivatedRoute) {
        this._tag = this._route.snapshot.paramMap.get("tag");
    }

    ngOnInit(): void {
        this._blogpostsService.getAll(this._tag).map(blogPosts => {
            let blogPostsViewModel: BlogPostViewModel[] = [];

            blogPosts.forEach(blogPost => {
                let blogPostViewModel = new BlogPostViewModel();
                blogPostViewModel.blogPost = blogPost;
                blogPostsViewModel.push(blogPostViewModel);
            });

            return blogPostsViewModel;
        }).subscribe(blogPosts => { this._blogPosts = blogPosts; });
    }
}

export class BlogPostViewModel {
    blogPost: BlogPostSummary;
    cssClass: string = "post";
}