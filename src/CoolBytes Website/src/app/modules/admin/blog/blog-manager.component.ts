import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { Author } from "../../../services/author";
import { AuthorsService } from "../../../services/authors.service";

@Component({
    templateUrl: "./blog-manager.component.html",
    styleUrls: ["./blog-manager.component.css"]
})
export class BlogManagerComponent implements OnInit {
    private author: Author;

    constructor(private _authorsService: AuthorsService, private _router: Router) {

    }

    ngOnInit(): void {
        this._authorsService.get().subscribe(
            author => this.author = author,
            (error: HttpErrorResponse) => {
                this._router.navigate(["admin/author"]);
            }
        )
    }
}