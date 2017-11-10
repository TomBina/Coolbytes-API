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
    _shareInfo;

    @Input()
    set shareInfo(value) {
        this._shareInfo = {};
        this._shareInfo.tweetMessage = encodeURIComponent(`${value.subject} - ${value.url}`);
        this._shareInfo.url = encodeURIComponent(value.url);
        this._shareInfo.whatsAppUrl = this._sanitizer.bypassSecurityTrustUrl(`whatsapp://send?text=${this._shareInfo.url}`);
        this._shareInfo.subject = encodeURIComponent(value.subject);
    }

    constructor(private _sanitizer: DomSanitizer) {

    }
}