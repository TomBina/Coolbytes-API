import { AddBlogComponent } from './blog/addblog/add-blog.component';
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { ReactiveFormsModule } from "@angular/forms";
import { Http, RequestOptions } from "@angular/http";

import { MenuComponent } from "./menu/menu.component";
import { ProcessAuthComponent } from "./processauth/process-auth.component";
import { BlogManagerComponent } from "./blog/blog-manager.component";

import { AuthService } from "../../services/auth.service";
import { AdminGuardService } from "../../services/admin-guard.service";
import { AuthorComponent } from "./author/author.component";
import { AuthorsService } from "../../services/authors.service";
import { AdminAuthorGuardService } from "../../services/admin-author-guard.service";

@NgModule({
    imports: [
        BrowserModule,
        RouterModule.forChild([
            {
                path: "processauth",
                component: ProcessAuthComponent
            },
            {
                path: "admin",
                component: BlogManagerComponent,
                canActivate: [AdminGuardService, AdminAuthorGuardService]
            },
            {
                path: "admin/blogs",
                component: BlogManagerComponent,
                canActivate: [AdminGuardService, AdminAuthorGuardService]
            },
            {
                path: "admin/blogs/add",
                component: AddBlogComponent,
                canActivate: [AdminGuardService, AdminAuthorGuardService]
            },
            {
                path: "admin/author",
                component: AuthorComponent,
                canActivate: [AdminGuardService]
            }
        ]),
        ReactiveFormsModule
    ],
    declarations: [
        MenuComponent,
        ProcessAuthComponent,
        BlogManagerComponent,
        AddBlogComponent,
        AuthorComponent
    ],
    providers: [
        AuthService,
        AuthorsService,
        AdminGuardService,
        AdminAuthorGuardService
    ]
})
export class AdminModule {

}