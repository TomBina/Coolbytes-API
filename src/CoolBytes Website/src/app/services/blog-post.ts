export class BlogPost {
    id: number;
    date: Date;
    updated: Date;
    subject: string;
    contentIntro: string;
    content: string;
    authorName: string;
    tags: BlogPostTag[];
}

export class BlogPostTag {
    id: number;
    name: string;
}