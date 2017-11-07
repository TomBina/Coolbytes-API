import { Component, Input } from "@angular/core";

@Component(
    {
        selector: "share",
        templateUrl: "./share.component.html",
        styleUrls: ["./share.component.css"]
    }
)

export class ShareComponent {
    private _shareInfo;

    @Input()
    set shareInfo(value) {
        this._shareInfo = {};
        this._shareInfo.tweetMessage = encodeURIComponent(`${value.subject} - ${value.url}`);
        this._shareInfo.url = encodeURIComponent(value.url);
        this._shareInfo.subject = encodeURIComponent(value.subject);
    }
}