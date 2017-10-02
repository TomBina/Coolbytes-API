import { Component, OnChanges, Input } from "@angular/core";
import * as marked from "marked";

@Component({
    selector: "md",
    template: `<div [innerHtml]="value"></div>`
})
export class Md implements OnChanges {
    @Input()
    value: string;

    ngOnChanges(): void {
        if (this.value)
            this.value = marked(this.value);
    }
}