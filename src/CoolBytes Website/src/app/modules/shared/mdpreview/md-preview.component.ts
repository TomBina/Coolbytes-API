import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
    selector: "admin-md-preview",
    templateUrl: "./md-preview.component.html"
})
export class MdPreviewComponent {
    @Input()
    text: string;
    private _previewOn: boolean;

    togglePreview() {
        this._previewOn = !this._previewOn;
    }
}