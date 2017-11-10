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
    isOpen;
    images: Image[];
    deleteImageText = "delete image";

    @Input()
    title: string;

    @Output()
    onImageSelected = new EventEmitter<Image>();
    
    private _imagesUri;
    
    constructor(public imagesService: ImagesService) { 
    }

    ngOnInit(): void {
        this.loadImages();
    }

    uploadImages(element: HTMLInputElement) {
        if (!element.files)
            return;

        this.imagesService.uploadImages(element.files).subscribe(images => {
            this.images = this.images.concat(images);
            element.value = "";
        })
    }

    loadImages() {
        this.imagesService.getAll().subscribe(images => this.images = images);
    }

    onImageClick(image: Image) {
        if (this.deleteImageText == "delete image")  {
            this.close();
            this.onImageSelected.emit(image);
        }
        else {
            this.imagesService.delete(image.id).subscribe(response => {
                this.loadImages();
            })
        }
    }

    toggleDelete() {
        if (this.deleteImageText == "delete image") {
            this.deleteImageText = "finished deleting";
        }
        else {
            this.deleteImageText = "delete image";
        }
    }

    open() {
        this.isOpen = true;
    }

    close() {
        this.isOpen = false;
    }
}