import { ResumeEvent } from '../../../../services/resumeservice/resume-event';
import { Component, Input } from "@angular/core";

@Component({
    selector: "home-about-resume-event",
    templateUrl: "./resume-event.component.html",
    styleUrls: ["./resume-event.component.css"]
})
export class ResumeEventComponent {
    @Input()
    resumeEvent: ResumeEvent
}