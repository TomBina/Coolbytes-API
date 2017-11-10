import { Author } from '../../../services/authorsservice/author';
import { Image } from '../../../services/imagesservice/image';
import { ImagesService } from '../../../services/imagesservice/images.service';
import { AuthorsService } from '../../../services/authorsservice/authors.service';
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, FormArray } from "@angular/forms";
import { Router } from "@angular/router";
import { Enumerate } from '../../../utils/enumerate';

@Component({
  templateUrl: "./author.component.html",
  styleUrls: ["./author.component.css"]
})
export class AuthorComponent implements OnInit {

  constructor(private _authorsService: AuthorsService, private _router: Router,
    private _imagesService: ImagesService, private _fb: FormBuilder) { }

  private _author: Author;
  imageUri: string;
  experiencesImageUris: any = {}
  form: FormGroup;

  ngOnInit() {
    this.initForm();
    this._authorsService.getWithProfile().subscribe(author => this.updateForm(author));
  }

  initForm() {
    this.form = this._fb.group({
      firstName: ["", [Validators.required, Validators.maxLength(50)]],
      lastName: ["", [Validators.required, Validators.maxLength(50)]],
      about: ["", [Validators.required, Validators.maxLength(500)]],
      imageId: [""],
      resumeUri: ["",[Validators.maxLength(255)]],
      linkedIn: ["",[Validators.maxLength(255)]],
      gitHub: ["",[Validators.maxLength(255)]],
      experiences: this._fb.array([])
    });
  }

  getExperiencesControls(): FormArray {
    return this.form.get("experiences") as FormArray;
  }

  addExperienceControl(): FormGroup {
    let experiences = this.getExperiencesControls();
    let experience = this.createExperienceFormGroup();
    experiences.push(experience);

    return experience;
  }

  createExperienceFormGroup(): FormGroup {
    return new FormGroup({
      id: new FormControl(""),
      name: new FormControl("", Validators.maxLength(50)),
      color: new FormControl("", [Validators.maxLength(6), Validators.minLength(6)]),
      imageId: new FormControl("")
    })
  }

  updateForm(author: Author) {
    Enumerate.deep(author, prop => this.form.get(prop) && prop != "experiences", (prop, value) => this.form.get(prop).setValue(value));
    this._author = author;

    if (author.image) {
      this.imageUri = this._imagesService.getUri(author.image.uriPath);
      this.form.get("imageId").setValue(author.image.id);
    }

    if (author.experiences) {
      let index = 0;
      author.experiences.forEach(e => {
        let control = this.addExperienceControl();
        control.get("id").setValue(e.id);
        control.get("name").setValue(e.name);
        control.get("color").setValue(e.color);
        control.get("imageId").setValue(e.image.id);
        this.experiencesImageUris[`Image${index}`] = this._imagesService.getUri(e.image.uriPath);
        index++;
      });
    }
  }

  removeExperience(index) {
    this.getExperiencesControls().removeAt(index);
  }

  onContentImageSelectedHandler(image: Image) {
    this.form.get("about").setValue(`${this.form.get("about").value}![](${this._imagesService.getUri(image.uriPath)})`);
  }

  onMainImageSelectedHandler(image: Image) {
    this.imageUri = this._imagesService.getUri(image.uriPath);
    this.form.get("imageId").setValue(image.id);
  }

  onExperiencesImageSelectedHandler(image: Image, formGroup: FormGroup, index: number) {
    this.experiencesImageUris[`Image${index}`] = this._imagesService.getUri(image.uriPath);
    formGroup.get("imageId").setValue(image.id);
  }

  onSubmit() {
    let controls = this.form.controls;

    if (!this.form.valid) {
      for (let controlName in controls) {
        controls[controlName].markAsTouched();
      }
      return;
    }

    let command: any = {};

    for (let controlName in controls) {
      if (controls[controlName].value && controls[controlName].value.length > 0)
        command[controlName] = controls[controlName].value;
    }

    if (this._author)
      this._authorsService.update(command).subscribe(author => this._router.navigate(["admin"]));
    else
      this._authorsService.add(command).subscribe(author => this._router.navigate(["admin"]));
  }
}