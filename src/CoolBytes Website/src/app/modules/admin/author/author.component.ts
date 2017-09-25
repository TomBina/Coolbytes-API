import { Router } from '@angular/router';
import { forEach } from "@angular/router/src/utils/collection";
import { Author } from "../../../services/author";
import { AuthorsService } from "../../../services/authors.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Component, OnInit } from "@angular/core";

@Component({
  templateUrl: "./author.component.html"
})
export class AuthorComponent implements OnInit {

  constructor(private _authorsService: AuthorsService, private _router: Router) { }

  authorForm: FormGroup;

  private _firstName: FormControl;
  private _lastName: FormControl;
  private _aboutMe: FormControl;

  ngOnInit() {
    this._firstName = new FormControl(null, [Validators.required, Validators.maxLength(50)]);
    this._lastName = new FormControl(null, [Validators.required, Validators.maxLength(50)]);
    this._aboutMe = new FormControl(null, [Validators.required, Validators.maxLength(500)]);

    this.authorForm = new FormGroup({
      firstName: this._firstName,
      lastName: this._lastName,
      aboutMe: this._aboutMe
    });

    this._authorsService.get().subscribe(
      author => {
        this._firstName.setValue(author.firstName);
        this._lastName.setValue(author.lastName);
        this._aboutMe.setValue(author.about);
      });
  }

  inputCssClass(name: string) {
    let formControl = this.authorForm.get(name);

    if (!formControl.valid && formControl.touched)
      return "error";
  }

  onSubmit() {
    if (!this.authorForm.valid) {
      for (let controlName in this.authorForm.controls) {
        this.authorForm.get(controlName).markAsTouched();
      }
      return;
    }

    var author = new Author();
    author.firstName = this._firstName.value;
    author.lastName = this._lastName.value;
    author.about = this._aboutMe.value;

    this._authorsService.add(author).subscribe(author => {
      this._router.navigate(["admin"]);
    });
  }
}
