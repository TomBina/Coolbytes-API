import { Image } from '../../../services/imagesservice/image';
import { ImagesService } from '../../../services/imagesservice/images.service';
import { AuthorsService } from '../../../services/authorsservice/authors.service';
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AddOrUpdateAuthorCommand } from '../../../services/authorsservice/add-or-update-author-command';

@Component({
  templateUrl: "./author.component.html"
})
export class AuthorComponent implements OnInit {

  constructor(private _authorsService: AuthorsService, private _router: Router, private _imagesService: ImagesService) { }

  authorForm: FormGroup;

  private _id: number;
  private _firstName: FormControl;
  private _lastName: FormControl;
  private _about: FormControl;
  private _image: Image;
  private _files: FileList;

  ngOnInit() {
    this._firstName = new FormControl(null, [Validators.required, Validators.maxLength(50)]);
    this._lastName = new FormControl(null, [Validators.required, Validators.maxLength(50)]);
    this._about = new FormControl(null, [Validators.required, Validators.maxLength(500)]);

    this.authorForm = new FormGroup({
      firstName: this._firstName,
      lastName: this._lastName,
      about: this._about
    });

    this._authorsService.get().subscribe(
      author => {
        this._id = author.id;
        this._firstName.setValue(author.firstName);
        this._lastName.setValue(author.lastName);
        this._about.setValue(author.about);
        this._image = author.image;

        if (this._image)
          this._image.uri = this._imagesService.getUri(this._image.uriPath);
      });
  }

  inputCssClass(name: string) {
    let formControl = this.authorForm.get(name);

    if (!formControl.valid && formControl.touched)
      return "error";
  }

  onFileChanged(element: HTMLInputElement) {
    this._files = element.files;
  }

  onSubmit() {
    if (!this.authorForm.valid) {
      for (let controlName in this.authorForm.controls) {
        this.authorForm.get(controlName).markAsTouched();
      }
      return;
    }

    let addOrUpdateAuthorCommand = new AddOrUpdateAuthorCommand();
    addOrUpdateAuthorCommand.firstName = this._firstName.value;
    addOrUpdateAuthorCommand.lastName = this._lastName.value;
    addOrUpdateAuthorCommand.about = this._about.value;
    addOrUpdateAuthorCommand.files = this._files;
    
    if (this._id) {
      this._authorsService.update(addOrUpdateAuthorCommand).subscribe(author => {
        this._router.navigate(["admin"]);
      });
    }
    else {
      this._authorsService.add(addOrUpdateAuthorCommand).subscribe(author => {
        this._router.navigate(["admin"]);
      });
    }
  }
}
