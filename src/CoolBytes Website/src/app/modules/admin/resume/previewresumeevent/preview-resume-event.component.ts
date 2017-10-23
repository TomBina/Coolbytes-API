import { Component, Input } from '@angular/core';

import { PreviewResumeEvent } from './preview-resume-event';

@Component({
    selector: 'admin-preview-resume-event',
    template: `<div *ngIf="previewResumeEvent">
                    <h1 class="no-top-margin">{{previewResumeEvent.name}}</h1>
                    <small>{{previewResumeEvent.startDate | date}} {{previewResumeEvent.endDate | date}}</small>
                    <md [value]="previewResumeEvent.message"></md>
               </div>
               <div *ngIf="!previewResumeEvent">
                Start typing to see preview.
               </div>`
})
export class PreviewResumeEventComponent {
    @Input()
    previewResumeEvent: PreviewResumeEvent;
}