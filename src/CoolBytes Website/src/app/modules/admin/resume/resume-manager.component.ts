import { AuthorsService } from '../../../services/authorsservice/authors.service';
import { Component, OnInit } from '@angular/core';

import { ResumeEvent } from '../../../services/resumeservice/resume-event';
import { ResumeEventsService } from '../../../services/resumeservice/resume-events.service';

@Component({ templateUrl: "./resume-manager.component.html", styleUrls: ["./resume-manager.component.css"] })
export class ResumeManagerComponent implements OnInit {
    resumeEvents: ResumeEvent[];

    constructor(private _resumeEventsService: ResumeEventsService, private _authorsService: AuthorsService) {

    }

    ngOnInit(): void {
        this.getResumeEvents();
    }

    getResumeEvents(): void {
        this._authorsService.get().subscribe(author => {
            this._resumeEventsService.getAll(author.id).subscribe(resumeEvents => this.resumeEvents = resumeEvents);
        })
    }

    delete(resumeEventId: number): void {
        if (!confirm("Are you sure?"))
            return;

        this._resumeEventsService.delete(resumeEventId).subscribe(response => {
            if (response.ok)
                this.getResumeEvents();
        });
    }
}