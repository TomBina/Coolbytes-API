import { Image } from "./image";
export class BlogPostSummary {
    id: number;
    date: Date;
    subject: string;
    subjectUrl: string;
    contentIntro: string;
    image: Image;
    authorName: string;
}