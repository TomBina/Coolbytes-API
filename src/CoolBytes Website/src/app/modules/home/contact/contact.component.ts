import { ContactService } from '../../../services/contactservice/contact.service';
import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators, FormGroup } from "@angular/forms";
import { Title } from '@angular/platform-browser';

@Component({
    templateUrl: "./contact.component.html",
    styleUrls: ["./contact.component.css"]
})
export class ContactComponent implements OnInit {
    form: FormGroup;
    isMessageSent: boolean;

    get messageSent() {
        return this.isMessageSent
    }

    set messageSent(value: boolean) {
        this.isMessageSent = value;
        for (let controlName in this.form.controls) {
            this.form.get(controlName).markAsUntouched();
            this.form.get(controlName).setValue("");
        }
        return;
    }

    constructor(private _fb: FormBuilder, private _contactService: ContactService, private _titleService: Title) {

    }

    ngOnInit() {
        this._titleService.setTitle("Contact Cool Bytes");
        this.form = this._fb.group({
            name: ["", Validators.required],
            email: ["", Validators.required],
            message: ["", [Validators.required, Validators.maxLength(2000)]]
        });
    }

    onSubmit() {
        var controls = this.form.controls;

        if (!this.form.valid) {
            for (let controlName in controls) {
                this.form.get(controlName).markAsTouched();
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