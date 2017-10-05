export class BlogPostPreview {
    subject: string;
    contentIntro: string;
    content: string;

    constructor(subject: string, contentIntro: string, content: string) {
        this.subject = subject;
        this.contentIntro = contentIntro;
        this.content = content;
    }
}