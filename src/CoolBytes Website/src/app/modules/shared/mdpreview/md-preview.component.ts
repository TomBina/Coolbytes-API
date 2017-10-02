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

    ngOnChanges(changes: SimpleChanges): void {
        this.text = marked(this.text);
    }

    togglePreview() {
        this._previewOn = !this._previewOn;
    }
}