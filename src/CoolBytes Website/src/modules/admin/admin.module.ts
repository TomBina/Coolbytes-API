import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { Http, RequestOptions } from "@angular/http";

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
            }
        ])
    ],
    declarations: [
        ProcessAuthComponent,
        BlogManagerComponent
    ],
    providers: [
        AuthService
    ]
})
export class AdminModule {

}