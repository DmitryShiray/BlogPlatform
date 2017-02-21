export class Comment {
    Id: number;
    Value: string;
    DateAdded: Date;
    ArticleId: number;
    AccountId: number;

    constructor(id: number, value: string, dateAdded: Date, articleId: number, accountId: number) {
        this.Id = id;
        this.Value = value;
        this.DateAdded = dateAdded;
        this.ArticleId = articleId;
        this.AccountId = accountId;
    }
}