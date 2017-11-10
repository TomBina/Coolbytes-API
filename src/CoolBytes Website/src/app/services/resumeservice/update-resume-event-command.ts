import { DateRange } from './date-range';
export class UpdateResumeEventCommand {
    id: number;
    dateRange: DateRange;
    name: string;
    message: string;
}