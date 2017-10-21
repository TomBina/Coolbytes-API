import { Author } from '../authorsservice/author';
import { Image } from '../imagesservice/image';
import { ExternalLink } from './external-link';
import { BlogPostLink } from './blog-post-link';
import { BlogPostTag } from './blog-post-tag';

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