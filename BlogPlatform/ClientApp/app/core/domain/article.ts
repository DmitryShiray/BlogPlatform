import { BaseProfile } from './baseProfile';
import { UtilityService } from '../services/utilityService';

const PREVIEWTEXTLENGTH: number = 1000;

export class Article {
    private previewText: string

    id: number;
    title: string;
    content: string;
    dateCreated: Date;
    accountId: number;
    totalComments: number;
    rating: number;
    author: BaseProfile;

    constructor(id: number, title: string, content: string, dateCreated: Date, accountId: number,
        totalComments: number, rating: number, author: BaseProfile) {
        this.id = id;
        this.title = title;
        this.content = content;
        this.dateCreated = dateCreated;
        this.accountId = accountId;
        this.totalComments = totalComments;
        this.rating = rating;
        this.author = author;
    }

    public getPreviewText(): string {
        if (!this.previewText || this.previewText.length === 0) {
            this.previewText = this.content.substring(0, PREVIEWTEXTLENGTH);
        }
        this.previewText = this.content.substring(0, PREVIEWTEXTLENGTH);
        return this.previewText;
    }
}