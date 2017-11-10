import { Component } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";


@Component({
    selector: "app-header",
    templateUrl: "./header.component.html",
    styleUrls: ["./header.component.css"]
})
export class HeaderComponent {
    constructor(public authService: AuthService, private _router: Router) {

    }

    toHome() {
        this._router.navigateByUrl("/home");
    }
}