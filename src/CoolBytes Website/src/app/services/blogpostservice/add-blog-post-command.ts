import { ExternalLink } from './external-link';
export class AddBlogPostCommand {
    subject: string;
    contentIntro: string;
    content: string;
    tags: string[];
    externalLinks: ExternalLink[];
}