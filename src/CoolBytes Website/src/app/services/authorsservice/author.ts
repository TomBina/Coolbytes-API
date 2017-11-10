import { Image } from '../imagesservice/image';
export class Author {
    id: number;
    firstName: string;
    lastName: string;
    about: string;
    image: Image;
    experiences: any;
}