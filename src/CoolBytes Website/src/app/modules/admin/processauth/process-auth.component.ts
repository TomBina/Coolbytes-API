import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../services/auth.service";

@Component({
    templateUrl: "./process-auth.component.html",
    styleUrls: ["./process-auth.component.css"]
})
export class ProcessAuthComponent implements OnInit {

    constructor(private _authService: AuthService) {

    }

    ngOnInit(): void {
        this._authService.handleAuthentication();
    }
}