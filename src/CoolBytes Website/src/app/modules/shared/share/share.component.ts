import { Component, Input } from "@angular/core";
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component(
    {
        selector: "share",
        templateUrl: "./share.component.html",
        styleUrls: ["./share.component.css"]
    }
)

export class ShareComponent {
    share;

    @Input()
    set shareInfo(value) {
        this.share = {};
        this.share.tweetMessage = encodeURIComponent(`${value.subject} - ${value.url}`);
        this.share.url = encodeURIComponent(value.url);
        this.share.whatsAppUrl = this._sanitizer.bypassSecurityTrustUrl(`whatsapp://send?text=${this.share.url}`);
        this.share.subject = encodeURIComponent(value.subject);
    }

    constructor(private _sanitizer: DomSanitizer) {

    }
}