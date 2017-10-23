import { ExternalLink } from './external-link';
export class UpdateBlogPostCommand {
    id: number;
    subject: string;
    contentIntro: string;
    content: string;
    tags: string[];
    externalLinks: ExternalLink[];
}