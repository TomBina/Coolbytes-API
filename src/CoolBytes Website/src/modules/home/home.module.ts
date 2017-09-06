import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";

import { BlogComponent } from "./blog/blog.component";
import { BlogPostComponent } from "./blogpost/blogpost.component";

@NgModule({
    imports: [
        BrowserModule,
        RouterModule.forChild([
            {
                path:"**",
                component:BlogComponent
            },
            {
                path:"home",
                component:BlogComponent
            }
        ])
    ],
    declarations: [
        BlogComponent,
        BlogPostComponent
    ]
})
export class HomeModule {

}