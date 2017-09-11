import { Component, OnInit } from "@angular/core";
import { DataService } from "../../../services/dataservice";
import { BlogPost } from "../../../services/blogpost";

@Component({
    templateUrl: "./blog.component.html",
    styleUrls: ["./blog.component.css"]
})
export class BlogComponent implements OnInit {
    blogPosts: BlogPost[];

    constructor(private _dataService: DataService) {
        
    }

    ngOnInit(): void {
        this._dataService.getBlogPosts().subscribe(blogPosts => {
            this.blogPosts = blogPosts;
        })
    }

}