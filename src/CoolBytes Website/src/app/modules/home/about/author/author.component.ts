import { environment } from '../../../../../environments/environment';
import { ImagesService } from '../../../../services/imagesservice/images.service';
import { Author } from '../../../../services/authorsservice/author';
import { Component, Input } from "@angular/core";

@Component(
    {
        selector: "home-about-author",
        templateUrl: "./author.component.html",
        styleUrls: ["./author.component.css"]
    }
)
export class AuthorComponent {
    experiences;
    authorImage: string;
    authorForTemplate: Author;

    @Input()
    set author(author: Author) {
        this.authorForTemplate = author;
        this.authorImage = this._imagesService.getUri(author.image.uriPath);

        if (author.experiences) {
            for (let experience of author.experiences) {
                experience.uri = `${environment.imagesUri}${experience.image.uriPath}`;
                experience.backgroundColor = `#${experience.color}`;
            }

            this.experiences = author.experiences;

        }
    }

    constructor(private _imagesService: ImagesService) {

    }
}