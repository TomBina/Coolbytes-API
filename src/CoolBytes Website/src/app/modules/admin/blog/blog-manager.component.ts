import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../services/auth.service";

@Component({
    templateUrl: "./blog-manager.component.html",
    styleUrls: ["./blog-manager.component.css"]
})
export class BlogManagerComponent implements OnInit {

    constructor(private _authService: AuthService) {

    }

    ngOnInit(): void {
        if (!this._authService.isAuthenticated())
            this._authService.login();
    }
}