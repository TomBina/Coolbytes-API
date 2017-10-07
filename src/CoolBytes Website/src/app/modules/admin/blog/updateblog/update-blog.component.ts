import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorsService } from '../../../../services/authors.service';
import { BlogPostSummary } from '../../../../services/blog-post-summary';
import { BlogPostUpdate } from '../../../../services/blog-post-update';
import { BlogPostUpdateCommand } from '../../../../services/blog-post-update-command';
import { BlogPostsService } from '../../../../services/blog-posts.service';
import { Image } from '../../../../services/image';
import { ImagesService } from '../../../../services/images.service';
import { BlogPostPreview } from '../previewblog/blog-post-preview';
import { PreviewBlogComponent } from '../previewblog/preview-blog.component';

@Component({
    templateUrl: "./update-blog.component.html",
    styleUrls: ["./update-blog.component.css"]
})
export class UpdateBlogComponent implements OnInit, OnDestroy {
    constructor(
        private _route: ActivatedRoute,
        private _authorsService: AuthorsService,
        private _blogPostsService: BlogPostsService,
        private _router: Router,
        private _imagesService: ImagesService) { }

    private _blogForm: FormGroup;
    private _id: number;
    private _blogPost: BlogPostSummary;
    private _subject: FormControl;
    private _contentIntro: FormControl;
    private _content: FormControl;
    private _tags: FormControl;
    private _image: Image;
    private _files: FileList;
    
    @ViewChild(PreviewBlogComponent)
    private _previewBlogComponent: PreviewBlogComponent;
    private _previewObserver: Subscription

    ngOnInit(): void {
        this._subject = new FormControl(null, [Validators.required, Validators.maxLength(100)]);
        this._contentIntro = new FormControl(null, [Validators.required, Validators.maxLength(100)]);
        this._content = new FormControl(null, [Validators.required, Validators.maxLength(4000)]);
        this._tags = new FormControl(null, Validators.maxLength(500));

        this._blogForm = new FormGroup({
            subject: this._subject,
            contentIntro: this._contentIntro,
            content: this._content,
            tags: this._tags
        });

        this._previewObserver = this._blogForm.valueChanges.subscribe(v => {
            this._previewBlogComponent.blogPostPreview = new BlogPostPreview(this._subject.value, this._contentIntro.value, this._content.value);
        })

        let id: number = parseInt(this._route.snapshot.paramMap.get("id"));
        this._blogPostsService.getUpdate(id).subscribe(blogPostUpdate => this.updateForm(blogPostUpdate));
    }

    ngOnDestroy(): void {
        if (this._previewObserver)
            this._previewObserver.unsubscribe();
    }

    updateForm(blogPost: BlogPostUpdate) {
        this._id = blogPost.id;
        this._subject.setValue(blogPost.subject);
        this._contentIntro.setValue(blogPost.contentIntro);
        this._content.setValue(blogPost.content);
        this._image = blogPost.image;

        if (this._image)
            this._image.uri = this._imagesService.getUri(this._image.uriPath);

        let blogPostTags: string[] = [];
        blogPost.tags.forEach(t => {
            blogPostTags.push(t.name);
        });

        this._tags.setValue(blogPostTags.join(","))
    }

    inputCssClass(name: string) {
        let formControl = this._blogForm.get(name);

        if (!formControl.valid && formControl.touched)
            return "error";
    }

    onFileChanged(element: HTMLInputElement) {
        this._files = element.files;
    }

    onSubmit(): void {
        if (!this._blogForm.valid) {
            for (let controlName in this._blogForm.controls) {
                this._blogForm.get(controlName).markAsTouched();
            }
            return;
        }

        let blogPostUpdate = new BlogPostUpdateCommand();

        blogPostUpdate.id = this._id;
        blogPostUpdate.subject = this._subject.value;
        blogPostUpdate.content = this._content.value;
        blogPostUpdate.contentIntro = this._contentIntro.value;

        let tags: string = this._tags.value;

        if (tags.indexOf(",") !== -1 || tags.length > 0) {
            blogPostUpdate.tags = tags.split(",");
        }

        this._blogPostsService.update(blogPostUpdate, this._files).subscribe(blogpost => {
            this._router.navigateByUrl("admin/blogs")
        });
    }
}