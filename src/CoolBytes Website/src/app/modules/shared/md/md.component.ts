import { ImagesService } from '../../../services/images.service';
import { Component, OnChanges, Input } from "@angular/core";
import * as marked from 'marked';

@Component({
    selector: "md",
    template: `<div [innerHtml]="value"></div>`
})
export class Md implements OnChanges {
    @Input()
    value: string;

    private _marked;

    constructor(private _imagesService: ImagesService) {
        let renderer = new marked.Renderer();
        renderer.image = (href: string, title: string, text: string) => {
            if (href.startsWith("/"))
                return `<img src="${_imagesService.getUri(href)}" />`
            else
                return `<img src="${href}" />`
        };
        marked.setOptions({ gfm: true, breaks: true, renderer: renderer });
        this._marked = marked;
    }

    ngOnChanges(): void {
        if (this.value)
            this.value = this._marked(this.value);
    }
}