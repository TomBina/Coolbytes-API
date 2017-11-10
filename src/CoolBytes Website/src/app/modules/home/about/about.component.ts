import { Author } from '../../../services/authorsservice/author';
import { ImagesService } from '../../../services/imagesservice/images.service';
import { ResumeEvent } from '../../../services/resumeservice/resume-event';
import { Resume } from '../../../services/resumeservice/resume';
import { ResumeService } from '../../../services/resumeservice/resume.service';
import { Component, OnInit } from "@angular/core";
import { Title } from '@angular/platform-browser';

@Component({ templateUrl: "./about.component.html", styleUrls: ["./about.component.css"] })
export class AboutComponent implements OnInit {
    resumeEvents;
    years: string[];
    author: Author;
    
    constructor(private _resumeService: ResumeService, private _imagesService: ImagesService, private _titleService: Title) { }

    ngOnInit() {
        this._titleService.setTitle("About Cool Bytes");
        this._resumeService.get(41).subscribe(resume => this.updateTemplate(resume));
    }

    updateTemplate(resume: Resume) {
        this.author = resume.author;
        this.resumeEvents = resume.resumeEvents;
        
        let years = [];
        
        for (let year in this.resumeEvents) {
            years.push(year);
        }
        
        this.years = years.sort(); 
    }
}