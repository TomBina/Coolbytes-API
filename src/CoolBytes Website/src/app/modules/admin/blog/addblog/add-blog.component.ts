import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorsService } from '../../../../services/authors.service';
import { BlogPostAddCommand } from '../../../../services/blog-post-add-command';
import { BlogPostsService } from '../../../../services/blog-posts.service';
import { ExternalLink } from '../../../../services/external-link';
import { BlogPostPreview } from '../previewblog/blog-post-preview';
import { PreviewBlogComponent } from '../previewblog/preview-blog.component';

@Component({
    templateUrl: "./add-blog.component.html",
    styleUrls: ["./add-blog.component.css"]
})
export class AddBlogComponent implements OnInit, OnDestroy {
    constructor(private _authorsService: AuthorsService, private _blogPostsService: BlogPostsService, private _router: Router, private _fb: FormBuilder) { }

    private _blogForm: FormGroup;
    private _externalLinks = [];
    private _files: FileList;

    @ViewChild(PreviewBlogComponent)
    private _previewBlogComponent: PreviewBlogComponent;
    private _previewObserver: Subscription

    ngOnInit() {
        this._blogForm = this._fb.group(
            {
                subject: ["", [Validators.required, Validators.maxLength(100)]],
                contentIntro: ["", [Validators.required, Validators.maxLength(100)]],
                content: ["", [Validators.required, Validators.maxLength(4000)]],
                tags: ["", [Validators.maxLength(500)]],
                externalLinks: this._fb.array([this.createExternalLinkFormGroup()])
            }
        )
        this._previewObserver = this._blogForm.valueChanges.subscribe(v => {
            this._previewBlogComponent.blogPostPreview
                = new BlogPostPreview(this._blogForm.get("subject").value,
                    this._blogForm.get("content").value,
                    this._blogForm.get("contentIntro").value);
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
        return this._blogForm.get("externalLinks") as FormArray;
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

        let blogPostAdd = new BlogPostAddCommand();
        blogPostAdd.subject = this._blogForm.get("subject").value;
        blogPostAdd.content = this._blogForm.get("content").value;
        blogPostAdd.contentIntro = this._blogForm.get("contentIntro").value;

        let externalLinks: ExternalLink[] = [];

        let controls = this.getExternalLinksControls();
        for (let control of controls.controls) {
            let externalLink = new ExternalLink(control.get("name").value, control.get("url").value);
            if (externalLink.name.length > 0 && externalLink.url.length > 0)
                externalLinks.push(externalLink);
        }

        blogPostAdd.externalLinks = externalLinks;

        let tags: string = this._blogForm.get("tags").value;
        if (tags.indexOf(",") !== -1 || tags.length > 0)
            blogPostAdd.tags = tags.split(",");

        this._blogPostsService.add(blogPostAdd, this._files).subscribe(blogpost => {
            this._router.navigateByUrl("admin/blogs")
        });
    }
}