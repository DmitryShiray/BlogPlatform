export class Article {
    Id: number;
    Title: string;
    Content: string;
    PreviewText: string;
    DateCreated: Date;
    AccountId: number;
    TotalComments: number;
    Rating: number;

    constructor(id: number, title: string, content: string, dateCreated: Date, accountId: number, totalComments: number, rating: number) {
        this.Id = id;
        this.Title = title;
        this.Content = content;
        this.PreviewText = content.substring(0, 100);
        this.DateCreated = dateCreated;
        this.AccountId = accountId;
        this.TotalComments = totalComments;
        this.Rating = rating;
    }
}