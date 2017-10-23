import { BaseProfile } from './baseProfile';

export class Rating {
    id: number;
    value: number;
    dateAdded: Date;
    articleId: number;
    author: BaseProfile;

    constructor(id: number, value: number, dateAdded: Date, articleId: number, author: BaseProfile) {
        this.id = id;
        this.value = value;
        this.dateAdded = dateAdded;
        this.articleId = articleId;
        this.author = author;
    }
}
