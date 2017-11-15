import { ImagesService } from '../../../../services/imagesservice/images.service';
import { Image } from '../../../../services/imagesservice/image';
import { AddBlogPostCommand } from '../../../../services/blogpostservice/add-blog-post-command';
import { ExternalLink } from '../../../../services/blogpostservice/external-link';
import { BlogPostsService } from '../../../../services/blogpostservice/blog-posts.service';
import { AuthorsService } from '../../../../services/authorsservice/authors.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { BlogPostPreview } from '../previewblog/blog-post-preview';
import { PreviewBlogComponent } from '../previewblog/preview-blog.component';

@Component({
    templateUrl: "./add-blog.component.html",
    styleUrls: ["./add-blog.component.css"]
})
export class AddBlogComponent implements OnInit, OnDestroy {
    constructor(private _authorsService: AuthorsService, private _blogPostsService: BlogPostsService, private _router: Router, private _fb: FormBuilder,
    private _imagesService : ImagesService) { }

    form: FormGroup;
    externalLinks = [];
    private _files: FileList;

    @ViewChild(PreviewBlogComponent)
    private _previewBlogComponent: PreviewBlogComponent;
    private _previewObserver: Subscription

    ngOnInit() {
        this.form = this._fb.group(
            {
                subject: ["", [Validators.required, Validators.maxLength(100)]],
                contentIntro: ["", [Validators.required, Validators.maxLength(120)]],
                content: ["", [Validators.required, Validators.maxLength(4000)]],
                tags: ["", [Validators.maxLength(500)]],
                externalLinks: this._fb.array([this.createExternalLinkFormGroup()])
            }
        )
        this._previewObserver = this.form.valueChanges.subscribe(v => {
            this._previewBlogComponent.blogPostPreview
                = new BlogPostPreview(this.form.get("subject").value,
                    this.form.get("contentIntro").value,
                    this.form.get("content").value);
        })
    }

    ngOnDestroy(): void {
        if (this._previewObserver)
            this._previewObserver.unsubscribe();
    }

    growTextarea(element: HTMLTextAreaElement) {
        element.style.height = `${element.scrollHeight+2}px`;
    }

    getExternalLinksControls(): FormArray {
        return this.form.get("externalLinks") as FormArray;
    }

    addExternalLinkControl() {
        let links = this.getExternalLinksControls();
        links.push(this.createExternalLinkFormGroup());
    }

    createExternalLinkFormGroup(): FormGroup {
        return new FormGroup({
            name: new FormControl("", Validators.maxLength(50)),
            url: new FormControl("", Validators.maxLength(255))
        })
    }

    onImageSelectedHandler(image: Image) {
        this.form.get("content").setValue(`${this.form.get("content").value}![](${this._imagesService.getUri(image.uriPath)})`);
    }

    onFileChanged(element: HTMLInputElement) {
        this._files = element.files;
    }

    onSubmit(): void {
        if (!this.form.valid) {
            for (let controlName in this.form.controls) {
                this.form.get(controlName).markAsTouched();
            }
            return;
        }

        let addBlogPostCommand = new AddBlogPostCommand();
        addBlogPostCommand.subject = this.form.get("subject").value;
        addBlogPostCommand.content = this.form.get("content").value;
        addBlogPostCommand.contentIntro = this.form.get("contentIntro").value;

        let externalLinks: ExternalLink[] = [];

        let controls = this.getExternalLinksControls();
        for (let control of controls.controls) {
            let externalLink = new ExternalLink(control.get("name").value, control.get("url").value);
            if (externalLink.name.length > 0 && externalLink.url.length > 0)
                externalLinks.push(externalLink);
        }

        addBlogPostCommand.externalLinks = externalLinks;

        let tags: string = this.form.get("tags").value;
        if (tags.indexOf(",") !== -1 || tags.length > 0)
            addBlogPostCommand.tags = tags.split(",");

        this._blogPostsService.add(addBlogPostCommand, this._files).subscribe(b => this._router.navigateByUrl("admin/blogs"));
    }
}