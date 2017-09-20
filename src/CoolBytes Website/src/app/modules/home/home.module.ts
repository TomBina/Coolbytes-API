import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { HttpModule } from "@angular/http";

import { BlogComponent } from "./blog.component";
import { BlogPostComponent } from "./blog-post.component";
import { BlogPostsService } from "../../services/blog-posts.service";

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
        BlogPostsService
    ]
})
export class HomeModule {

}