import { ExternalLink } from './external-link';
export class BlogPostAddCommand {
    subject: string;
    contentIntro: string;
    content: string;
    tags: string[];
    externalLinks: ExternalLink[];
}