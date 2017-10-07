import { BlogPostTag } from './blog-post-tag';
import { Image } from "./image";

export class BlogPostUpdate {
    id: number;
    date: Date;
    updated: Date;
    subject: string;
    contentIntro: string;
    content: string;
    tags: BlogPostTag[];
    image: Image;
}