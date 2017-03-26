export class Rating {
    Id: number;
    Value: number;
    DateAdded: Date;
    ArticleId: number;
    AccountId: number;

    constructor(id: number, value: number, dateAdded: Date, articleId: number, accountId: number) {
        this.Id = id;
        this.Value = value;
        this.DateAdded = dateAdded;
        this.ArticleId = articleId;
        this.AccountId = accountId;
    }
}