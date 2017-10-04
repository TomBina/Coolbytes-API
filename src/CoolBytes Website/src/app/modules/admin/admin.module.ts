import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AdminAuthorGuardService } from '../../services/admin-author-guard.service';
import { AdminGuardService } from '../../services/admin-guard.service';
import { AuthService } from '../../services/auth.service';
import { AuthorsService } from '../../services/authors.service';
import { ImagesService } from '../../services/images.service';
import { SharedModule } from '../shared/shared.module';
import { AuthorComponent } from './author/author.component';
import { AddBlogComponent } from './blog/addblog/add-blog.component';
import { BlogManagerComponent } from './blog/blog-manager.component';
import { UpdateBlogComponent } from './blog/updateblog/update-blog.component';
import { ImagesManagerComponent } from './images/images-manager.component';
import { MenuComponent } from './menu/menu.component';
import { ProcessAuthComponent } from './processauth/process-auth.component';

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
                path: "admin/blogs/edit/:id",
                component: UpdateBlogComponent,
                canActivate: [AdminGuardService, AdminAuthorGuardService]
            },
            {
                path: "admin/author",
                component: AuthorComponent,
                canActivate: [AdminGuardService]
            }
        ]),
        ReactiveFormsModule,
        SharedModule
    ],
    declarations: [
        MenuComponent,
        ProcessAuthComponent,
        BlogManagerComponent,
        AddBlogComponent,
        UpdateBlogComponent,
        AuthorComponent,
        ImagesManagerComponent
    ],
    providers: [
        AuthService,
        AuthorsService,
        ImagesService,
        AdminGuardService,
        AdminAuthorGuardService
    ]
})
export class AdminModule {

}