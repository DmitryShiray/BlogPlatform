import { Component, OnInit } from '@angular/core';
import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'articles',
    template: require('./articles.component.html'),
    styles: [require('./articles.component.css')],
    providers: [UtilityService, DataService, NotificationService]
})

export class ArticlesComponent implements OnInit {
    private articlesReadUrl: string = 'api/articles/';
    private articles: Array<Article>;

    constructor(public articlesService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService) {
        this.articles = [];
    }

    ngOnInit() {
        this.articlesService.set(this.articlesReadUrl);
        this.getArticles();
    }

    getArticles(): void {
        this.articlesService.get(1)
            .subscribe(res => {
                var data: any = res.json();
                for (let i = 0; i < data.length; i++) {
                    var account = data[i].account;
                    let author = new BaseProfile(account["firstName"], account["lastName"], account["nickname"], account["emailAddress"]);
                    this.articles.push(new Article(
                        data[i]["id"],
                        data[i]["title"],
                        data[i]["content"],
                        data[i]["dateCreated"],
                        data[i]["accountId"],
                        data[i]["totalComments"],
                        data[i]["rating"],
                        author));
                }
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }

    search(articleId): void {
        this.getArticles();
    };

    public convertDateTime(date: Date) {
        return this.utilityService.convertDateTimeToString(date);
    }

    public showNickname(article: Article): boolean {
        var author = article.author;
        return author.nickname && author.nickname.length !== 0;
    }
}