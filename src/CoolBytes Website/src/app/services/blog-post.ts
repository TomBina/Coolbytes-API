import { ExternalLink } from './external-link';
import { BlogPostLink } from './blog-post-link';
import { Author } from './author';
import { BlogPostTag } from './blog-post-tag';
import { Image } from "./image";

export class BlogPost {
    id: number;
    date: Date;
    updated: Date;
    subject: string;
    contentIntro: string;
    content: string;
    tags: BlogPostTag[];
    image: Image;
    author: Author;
    links: BlogPostLink[];
    externalLinks: ExternalLink[];
}