import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { Http, RequestOptions } from "@angular/http";

import { MenuComponent } from "./menu/menu.component";
import { ProcessAuthComponent } from "./processauth/process-auth.component";
import { BlogManagerComponent } from "./blog/blog-manager.component";

import { AuthService } from "../../services/auth.service";
import { AdminGuardService } from '../../services/admin-guard.service';

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
                canActivate: [AdminGuardService]
            },
            {
                path: "admin/blogs",
                component: BlogManagerComponent,
                canActivate: [AdminGuardService]
            }
        ])
    ],
    declarations: [
        MenuComponent,
        ProcessAuthComponent,
        BlogManagerComponent
    ],
    providers: [
        AuthService,
        AdminGuardService
    ]
})
export class AdminModule {

}