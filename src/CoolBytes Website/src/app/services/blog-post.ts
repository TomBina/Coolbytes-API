import { Image } from "./image";
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
    image: Image;
}

export class BlogPostTag {
    id: number;
    name: string;
}