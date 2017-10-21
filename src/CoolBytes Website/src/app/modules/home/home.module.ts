import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { SharedModule } from "../shared/shared.module";
import { BlogPostComponent } from "./blog-post.component";
import { NgModule } from "@angular/core";
import { HttpModule } from "@angular/http";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";

import { BlogPostIntroComponent } from "./blog-post-intro.component";
import { BlogComponent } from "./blog.component";

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
            },
            {
                path:"post/:id/:title",
                component:BlogPostComponent
            }
        ]),
        HttpModule,
        SharedModule
    ],
    declarations: [
        BlogComponent,
        BlogPostIntroComponent,
        BlogPostComponent
    ],
    providers: [
        BlogPostsService
    ]
})
export class HomeModule {

}