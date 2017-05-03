import { BaseProfile } from './baseProfile';

export class Comment {
    id: number;
    text: string;
    dateAdded: Date;
    articleId: number;
    author: BaseProfile;

    constructor(id: number, text: string, dateAdded: Date, articleId: number, author: BaseProfile) {
        this.id = id;
        this.text = text;
        this.dateAdded = dateAdded;
        this.articleId = articleId;
        this.author = author;
    }
}