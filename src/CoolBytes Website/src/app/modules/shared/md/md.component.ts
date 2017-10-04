import { Component, OnChanges, Input } from "@angular/core";
import * as marked from "marked";

@Component({
    selector: "md",
    template: `<div [innerHtml]="value"></div>`
})
export class Md implements OnChanges {
    @Input()
    value: string;

    private _marked;

    constructor() {
        this._marked = marked;
        marked.setOptions({ gfm:true, breaks: true });
    }

    ngOnChanges(): void {
        if (this.value)
            this.value = this._marked(this.value);
    }
}