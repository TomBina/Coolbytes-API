import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../services/authservice";

@Component({
    templateUrl: "./blogmanager.component.html",
    styleUrls: ["./blogmanager.component.css"]
})
export class BlogManagerComponent implements OnInit {

    constructor(private _authService: AuthService) {

    }

    ngOnInit(): void {
        if (!this._authService.isAuthenticated())
            this._authService.login();
    }
}