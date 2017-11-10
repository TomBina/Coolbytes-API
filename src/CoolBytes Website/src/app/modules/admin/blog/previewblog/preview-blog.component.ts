import { BlogPostPreview } from './blog-post-preview';
import { Component, Input, OnChanges } from '@angular/core';

@Component(
    {
        selector: 'admin-preview-blog',
        template: `<div *ngIf="blogPostPreview">
                        <h1 class="no-top-margin">{{blogPostPreview.subject}}</h1>
                        <md [value]="blogPostPreview.contentIntro"></md>
                        <md [value]="blogPostPreview.content"></md>
                   </div>
                   <div *ngIf="!blogPostPreview">
                    Start typing to see preview.
                   </div>`
    }
)
export class PreviewBlogComponent {
    @Input()
    blogPostPreview: BlogPostPreview
}