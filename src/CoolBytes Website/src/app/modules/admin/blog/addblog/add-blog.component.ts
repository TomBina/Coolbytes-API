import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthorsService } from '../../../../services/authors.service';
import { BlogPostAdd } from '../../../../services/blog-post-add';
import { BlogPostsService } from '../../../../services/blog-posts.service';

@Component({
    templateUrl: "./add-blog.component.html",
    styleUrls: ["./add-blog.component.css"]
})
export class AddBlogComponent implements OnInit {

    constructor(private _authorsService: AuthorsService, private _blogPostsService: BlogPostsService, private _router: Router) { }

    blogForm: FormGroup;

    private _subject: FormControl;
    private _contentIntro: FormControl;
    private _content: FormControl;
    private _tags: FormControl;
    private _files: FileList;

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
    }

    inputCssClass(name: string) {
        let formControl = this.blogForm.get(name);

        if (!formControl.valid && formControl.touched)
            return "error";
    }

    onFileChanged(element: HTMLInputElement) {
        this._files = element.files;
    }

    onSubmit(): void {
        if (!this.blogForm.valid) {
            for (let controlName in this.blogForm.controls) {
                this.blogForm.get(controlName).markAsTouched();
            }
            return;
        }

        let blogPostAdd = new BlogPostAdd();

        blogPostAdd.subject = this._subject.value;
        blogPostAdd.content = this._content.value;
        blogPostAdd.contentIntro = this._contentIntro.value;

        let tags: string = this._tags.value;
        if (tags.indexOf(",") !== -1 || tags.length > 0)
            blogPostAdd.tags = tags.split(",");

        this._blogPostsService.add(blogPostAdd, this._files).subscribe(blogpost => {
            this._router.navigateByUrl("admin/blogs")
        });
    }
}