import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { Http, RequestOptions } from "@angular/http";

import { MenuComponent } from "./menu/menu.component";
import { ProcessAuthComponent } from "./processauth/processauth.component";
import { BlogManagerComponent } from "./blog/blogmanager.component";

import { AuthService } from "../../services/authservice";

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
                component: BlogManagerComponent
            },
            {
                path: "admin/blogs",
                component: BlogManagerComponent
            }
        ])
    ],
    declarations: [
        MenuComponent,
        ProcessAuthComponent,
        BlogManagerComponent
    ],
    providers: [
        AuthService
    ]
})
export class AdminModule {

}