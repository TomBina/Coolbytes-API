import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { HttpModule } from "@angular/http";

import { BlogComponent } from "./blog/blog.component";
import { BlogPostComponent } from "./blogpost/blogpost.component";
import { DataService } from "../../services/dataservice";

@NgModule({
    imports: [
        BrowserModule,
        RouterModule.forChild([
            {
                path:"",
                component:BlogComponent
            },
            {
                path:"home",
                component:BlogComponent
            }
        ]),
        HttpModule
    ],
    declarations: [
        BlogComponent,
        BlogPostComponent
    ],
    providers: [
        DataService
    ]
})
export class HomeModule {

}