import * as marked from "marked"
import { Component, Input, OnChanges, SimpleChanges } from "@angular/core";

@Component({
    selector: "admin-md-preview",
    templateUrl: "./md-preview.component.html"
})
export class MdPreviewComponent implements OnChanges {
    @Input()
    text: string;
    private _previewOn: boolean;
    private _marked;

    constructor() {
        this._marked = marked;
        marked.setOptions({ gfm:true, breaks: true });
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.text = this._marked(this.text);
    }

    togglePreview() {
        this._previewOn = !this._previewOn;
    }
}