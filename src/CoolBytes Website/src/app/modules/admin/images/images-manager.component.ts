import { ImagesService } from '../../../services/imagesservice/images.service';
import { Image } from '../../../services/imagesservice/image';
import { environment } from '../../../../environments/environment';
import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormControl } from "@angular/forms";

@Component({
    selector: "admin-images-manager",
    templateUrl: "./images-manager.component.html",
    styleUrls: ["./images-manager.component.css"]
})
export class ImagesManagerComponent implements OnInit {
    _open;
    _images: Image[];
    _imagesUri;
    _deleteImageText = "delete image";

    @Input()
    title: string;

    @Output()
    onImageSelected = new EventEmitter<Image>();
    
    constructor(private _imagesService: ImagesService) { 
    }

    ngOnInit(): void {
        this.loadImages();
    }

    uploadImages(element: HTMLInputElement) {
        if (!element.files)
            return;

        this._imagesService.uploadImages(element.files).subscribe(images => {
            this._images = this._images.concat(images);
            element.value = "";
        })
    }

    loadImages() {
        this._imagesService.getAll().subscribe(images => this._images = images);
    }

    onImageClick(image: Image) {
        if (this._deleteImageText == "delete image")  {
            this.close();
            this.onImageSelected.emit(image);
        }
        else {
            this._imagesService.delete(image.id).subscribe(response => {
                this.loadImages();
            })
        }
    }

    toggleDelete() {
        if (this._deleteImageText == "delete image") {
            this._deleteImageText = "finished deleting";
        }
        else {
            this._deleteImageText = "delete image";
        }
    }

    open() {
        this._open = true;
    }

    close() {
        this._open = false;
    }
}