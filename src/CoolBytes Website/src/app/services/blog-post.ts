import { Photo } from "./photo";
export class BlogPost {
    id: number;
    date: Date;
    updated: Date;
    subject: string;
    subjectUrl: string;
    contentIntro: string;
    content: string;
    authorName: string;
    tags: BlogPostTag[];
    photo: Photo;
}

export class BlogPostTag {
    id: number;
    name: string;
}