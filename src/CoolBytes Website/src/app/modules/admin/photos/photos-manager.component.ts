import { Photo } from "../../../services/photo";
import { PhotosService } from "../../../services/photos.service";
import { Component, OnInit, Input } from "@angular/core";
import { FormControl } from "@angular/forms";

@Component({
    selector: "admin-photos-manager",
    templateUrl: "./photos-manager.component.html",
    styleUrls: ["./photos-manager.component.css"]
})
export class PhotosManagerComponent implements OnInit {
    constructor(private _photoService: PhotosService) { }

    private _open;
    private _photos: Photo[];

    @Input()
    control: FormControl;

    ngOnInit(): void {
        this.loadPhotos();
    }

    uploadPhotos(element: HTMLInputElement) {
        if (!element.files)
            return;

        this._photoService.uploadPhotos(element.files).subscribe(photos => {
            this._photos = this._photos.concat(photos);
            element.value = "";
        })
    }

    loadPhotos() {
        this._photoService.getAll().subscribe(photos => this._photos = photos);
    }

    insertPhoto(uri: string, id: number) {
        let value: string = (this.control.value ? this.control.value : "");
        value += `![](${uri})`;

        this.control.setValue(value);
    }

    open() {
        this._open = true;
    }

    close() {
        this._open = false;
    }
}