import { Image } from '../imagesservice/image';
import { ExternalLink } from './external-link';
import { BlogPostTag } from './blog-post-tag';

export class BlogPostUpdate {
    id: number;
    date: Date;
    updated: Date;
    subject: string;
    contentIntro: string;
    content: string;
    tags: BlogPostTag[];
    image: Image;
    externalLinks: ExternalLink[];
}