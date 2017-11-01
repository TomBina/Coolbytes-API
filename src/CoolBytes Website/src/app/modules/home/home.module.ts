import { AuthorComponent } from './about/author/author.component';
import { ResumeEventComponent } from './about/resumeevent/resume-event.component';
import { ResumeService } from '../../services/resumeservice/resume.service';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { BlogPostsService } from '../../services/blogpostservice/blog-posts.service';
import { SharedModule } from '../shared/shared.module';
import { AboutComponent } from './about/about.component';
import { BlogPostIntroComponent } from './blog-post-intro.component';
import { BlogPostComponent } from './blog-post.component';
import { BlogComponent } from './blog.component';

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
            },
            {
                path:"about",
                component:AboutComponent
            }
        ]),
        HttpModule,
        SharedModule
    ],
    declarations: [
        BlogComponent,
        BlogPostIntroComponent,
        BlogPostComponent,
        AboutComponent,
        ResumeEventComponent,
        AuthorComponent
    ],
    providers: [
        BlogPostsService,
        ResumeService
    ]
})
export class HomeModule {

}