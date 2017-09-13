import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../services/authservice";

@Component({
    templateUrl: "./processauth.component.html",
    styleUrls: ["./processauth.component.css"]
})
export class ProcessAuthComponent implements OnInit {

    constructor(private _authService: AuthService) {

    }

    ngOnInit(): void {
        this._authService.handleAuthentication();
    }
}