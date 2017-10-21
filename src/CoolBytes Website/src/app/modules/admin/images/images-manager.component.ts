import { ImagesService } from '../../../services/imagesservice/images.service';
import { Image } from '../../../services/imagesservice/image';
import { environment } from '../../../../environments/environment';
import { Component, OnInit, Input } from "@angular/core";
import { FormControl } from "@angular/forms";

@Component({
    selector: "admin-images-manager",
    templateUrl: "./images-manager.component.html",
    styleUrls: ["./images-manager.component.css"]
})
export class ImagesManagerComponent implements OnInit {
    private _open;
    private _images: Image[];
    private _imagesUri;
    
    constructor(private _imageService: ImagesService) { 
    }

    @Input()
    control: FormControl;

    ngOnInit(): void {
        this.loadImages();
    }

    uploadImages(element: HTMLInputElement) {
        if (!element.files)
            return;

        this._imageService.uploadImages(element.files).subscribe(images => {
            this._images = this._images.concat(images);
            element.value = "";
        })
    }

    loadImages() {
        this._imageService.getAll().subscribe(images => this._images = images);
    }

    insertImage(uri: string, id: number) {
        let value: string = (this.control.value ? this.control.value : "");
        value += `![](${uri})`;

        this.control.setValue(value);
        this.close();
    }

    open() {
        this._open = true;
    }

    close() {
        this._open = false;
    }
}