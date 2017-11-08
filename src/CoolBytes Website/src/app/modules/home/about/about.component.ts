import { Author } from '../../../services/authorsservice/author';
import { ImagesService } from '../../../services/imagesservice/images.service';
import { ResumeEvent } from '../../../services/resumeservice/resume-event';
import { Resume } from '../../../services/resumeservice/resume';
import { ResumeService } from '../../../services/resumeservice/resume.service';
import { Component, OnInit } from "@angular/core";

@Component({ templateUrl: "./about.component.html", styleUrls: ["./about.component.css"] })
export class AboutComponent implements OnInit {
    _resumeEvents;
    _years: string[];
    _author: Author;
    
    constructor(private _resumeService: ResumeService, private _imagesService: ImagesService) { }

    ngOnInit() {
        this._resumeService.get(41).subscribe(resume => this.updateTemplate(resume));
    }

    updateTemplate(resume: Resume) {
        this._author = resume.author;
        this._resumeEvents = resume.resumeEvents;
        
        let years = [];
        
        for (let year in this._resumeEvents) {
            years.push(year);
        }
        
        this._years = years.sort(); 
    }
}