import { ImagesService } from '../../../services/images.service';
import { Component, OnChanges, Input } from "@angular/core";
import * as marked from 'marked';
import * as highlight from "../../../../external/highlightjs";
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
    selector: "md",
    template: `<div [innerHtml]="html"></div>`
})
export class MdComponent implements OnChanges {
    @Input()
    value: string;
    html: SafeHtml;

    private _marked;

    constructor(private _imagesService: ImagesService, private _sanitizer: DomSanitizer) {
        let renderer = new marked.Renderer();
        renderer.image = (href: string, title: string, text: string) => {
            if (href.startsWith("/"))
                return `<img src="${_imagesService.getUri(href)}" />`
            else
                return `<img src="${href}" />`
        };
        marked.setOptions({
            gfm: true,
            breaks: true,
            renderer: renderer,
            sanitize: true,
            highlight: (c, lang) =>  {
                let val = highlight.highlight("cs", c).value;
                return `<div class="hljs">${highlight.highlight("cs", c).value}</div>`
            }
        });
        this._marked = marked;
    }

    ngOnChanges(): void {
        if (this.value)
            this.html = this._sanitizer.bypassSecurityTrustHtml(this._marked(this.value));
    }
}