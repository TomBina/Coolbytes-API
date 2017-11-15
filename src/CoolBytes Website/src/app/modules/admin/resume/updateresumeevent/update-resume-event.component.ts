import { ResumeEvent } from '../../../../services/resumeservice/resume-event';
import { ResumeEventsService } from '../../../../services/resumeservice/resume-events.service';
import { DateRange } from '../../../../services/resumeservice/date-range';
import { UpdateResumeEventCommand } from '../../../../services/resumeservice/update-resume-event-command';
import { Component, ViewChild } from "@angular/core";
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { PreviewResumeEvent } from '../previewresumeevent/preview-resume-event';
import { PreviewResumeEventComponent } from '../previewresumeevent/preview-resume-event.component';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: "./update-resume-event.component.html",
    styleUrls: ["./update-resume-event.component.css"]
})
export class UpdateResumeEventComponent {
    form: FormGroup;

    @ViewChild(PreviewResumeEventComponent)
    private _previewResumeEvent: PreviewResumeEventComponent;
    private _previewObserver: Subscription
    private _id: number;

    constructor(private _fb: FormBuilder, private _resumeService: ResumeEventsService, private _router: Router, private _route: ActivatedRoute) {

    }

    ngOnInit() {
        this.form = this._fb.group({
            startDate: ["", [Validators.required]],
            endDate: ["", [Validators.required]],
            name: ["", [Validators.required, Validators.maxLength(50)]],
            message: ["", [Validators.required, Validators.maxLength(1000)]]
        });

        this._previewObserver = this.form.valueChanges.subscribe(v => {
            let previewResumeEvent = new PreviewResumeEvent();
            previewResumeEvent.startDate = this.form.get("startDate").value;
            previewResumeEvent.endDate = this.form.get("endDate").value;
            previewResumeEvent.name = this.form.get("name").value;
            previewResumeEvent.message = this.form.get("message").value;

            this._previewResumeEvent.previewResumeEvent = previewResumeEvent;
        })

        let id: number = parseInt(this._route.snapshot.paramMap.get("id"));
        this._resumeService.get(id).subscribe(resumeEvent => {
            this._id = resumeEvent.id;
            this.updateForm(resumeEvent);
        });
    }

    formatDate(jsonDate: string) {
        let date = new Date(jsonDate);

        return `${date.getMonth()+1}-${date.getDate()}-${date.getFullYear()}`;
    }
    updateForm(resumeEvent: ResumeEvent) {
        this.form.get("startDate").setValue(this.formatDate(resumeEvent.dateRange.startDate));
        this.form.get("endDate").setValue(this.formatDate(resumeEvent.dateRange.endDate));
        this.form.get("name").setValue(resumeEvent.name);
        this.form.get("message").setValue(resumeEvent.message);
    }

    ngOnDestroy(): void {
        if (this._previewObserver)
            this._previewObserver.unsubscribe();
    }

    growTextarea(element: HTMLTextAreaElement) {
        element.style.height = `${element.scrollHeight + 2}px`;
    }

    onSubmit() {
        if (!this.form.valid) {
            for (let controlName in this.form.controls) {
                this.form.get(controlName).markAsTouched();
            }
            return;
        }

        var updateResumeEventCommand = new UpdateResumeEventCommand();
        var dateRange = new DateRange();

        dateRange.startDate = this.form.get("startDate").value;
        dateRange.endDate = this.form.get("endDate").value;
        updateResumeEventCommand.id = this._id;
        updateResumeEventCommand.dateRange = dateRange;
        updateResumeEventCommand.name = this.form.get("name").value;
        updateResumeEventCommand.message = this.form.get("message").value;

        this._resumeService.update(updateResumeEventCommand).subscribe(r => this._router.navigateByUrl("admin/resume"));
    }
}