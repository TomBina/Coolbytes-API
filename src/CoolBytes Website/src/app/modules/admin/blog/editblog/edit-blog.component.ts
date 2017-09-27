import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthorsService } from '../../../../services/authors.service';
import { BlogPost } from '../../../../services/blog-post';
import { BlogPostUpdate } from '../../../../services/blog-post-update';
import { BlogPostsService } from '../../../../services/blog-posts.service';

@Component({
    templateUrl: "./edit-blog.component.html",
    styleUrls: ["./edit-blog.component.css"]
})
export class EditBlogComponent implements OnInit {

    constructor(
        private _route: ActivatedRoute,
        private _authorsService: AuthorsService,
        private _blogPostsService: BlogPostsService,
        private _router: Router) { }

    blogForm: FormGroup;

    private _id: number;
    private _blogPost: BlogPost;
    private _subject: FormControl;
    private _contentIntro: FormControl;
    private _content: FormControl;
    private _tags: FormControl;

    ngOnInit(): void {
        this._subject = new FormControl(null, [Validators.required, Validators.maxLength(100)]);
        this._contentIntro = new FormControl(null, [Validators.required, Validators.maxLength(100)]);
        this._content = new FormControl(null, [Validators.required, Validators.maxLength(4000)]);
        this._tags = new FormControl(null, Validators.maxLength(500));

        this.blogForm = new FormGroup({
            subject: this._subject,
            contentIntro: this._contentIntro,
            content: this._content,
            tags: this._tags
        });

        let id: number = parseInt(this._route.snapshot.paramMap.get("id"));

        this._blogPostsService.get(id).subscribe(blogPost => this.updateForm(blogPost));
    }

    updateForm(blogPost: BlogPost) {
        this._id = blogPost.id;
        this._subject.setValue(blogPost.subject);
        this._contentIntro.setValue(blogPost.contentIntro);
        this._content.setValue(blogPost.content);

        let blogPostTags: string[] = [];
        blogPost.tags.forEach(t => {
            blogPostTags.push(t.name);
        });

        this._tags.setValue(blogPostTags.join(","))
    }

    inputCssClass(name: string) {
        let formControl = this.blogForm.get(name);

        if (!formControl.valid && formControl.touched)
            return "error";
    }

    onSubmit(): void {
        if (!this.blogForm.valid) {
            for (let controlName in this.blogForm.controls) {
                this.blogForm.get(controlName).markAsTouched();
            }
            return;
        }

        let blogPostUpdate = new BlogPostUpdate();

        blogPostUpdate.id = this._id;
        blogPostUpdate.subject = this._subject.value;
        blogPostUpdate.content = this._content.value;
        blogPostUpdate.contentIntro = this._contentIntro.value;

        let tags: string = this._tags.value;

        if (tags.indexOf(",") !== -1 || tags.length > 0) {
            blogPostUpdate.tags = tags.split(",");
        }

        this._blogPostsService.update(blogPostUpdate).subscribe(blogpost => {
            this._router.navigateByUrl("admin/blogs")
        });
    }
}