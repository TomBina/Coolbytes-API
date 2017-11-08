import { ContactService } from '../../../services/contactservice/contact.service';
import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators, FormGroup } from "@angular/forms";

@Component({
    templateUrl: "./contact.component.html",
    styleUrls: ["./contact.component.css"]
})
export class ContactComponent implements OnInit {
    _form: FormGroup;
    _messageSent: boolean;

    get messageSent() {
        return this._messageSent
    }

    set messageSent(value: boolean) {
        this._messageSent = value;
        for (let controlName in this._form.controls) {
            this._form.get(controlName).markAsUntouched();
            this._form.get(controlName).setValue("");
        }
        return;
    }

    constructor(private _fb: FormBuilder, private _contactService: ContactService) {

    }

    ngOnInit() {
        this._form = this._fb.group({
            name: ["", Validators.required],
            email: ["", Validators.required],
            message: ["", [Validators.required, Validators.maxLength(2000)]]
        });
    }

    onSubmit() {
        var controls = this._form.controls;

        if (!this._form.valid) {
            for (let controlName in controls) {
                this._form.get(controlName).markAsTouched();
            }
            return;
        }

        let command: any = {};

        for (let controlName in controls) {
            if (controls[controlName].value && controls[controlName].value.length > 0)
                command[controlName] = controls[controlName].value;
        }

        this._contactService.Send(command).subscribe(response => this.messageSent = response.ok);
    }
}