import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'article',
    template: require('./article.component.html'),
    styles: [require('./article.component.css')],
    providers: [UtilityService, DataService, NotificationService]
})

export class ArticleComponent implements OnInit {
    private articlesReadUrl: string = 'api/articles/article/';
    private article: Article;

    constructor(public articlesService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService,
        private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        this.articlesService.set(this.articlesReadUrl);
        this.getArticle();
    }

    getArticle(): void {
        let articleId = this.activatedRoute.snapshot.params['articleId'];

        this.articlesService.getItem(articleId)
            .subscribe(res => {
                let data: any = res.json();
                let account = data["account"];
                let author = new BaseProfile(account["firstName"], account["lastName"], account["nickname"], account["emailAddress"]);
                this.article = new Article(
                    data["id"],
                    data["title"],
                    data["content"],
                    data["dateCreated"],
                    data["accountId"],
                    data["totalComments"],
                    data["rating"],
                    author);
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }

    public convertDateTime(date: Date) {
        return this.utilityService.convertDateTimeToString(date);
    }

    public showNickname(article: Article): boolean {
        var author = article.author;
        return author.nickname && author.nickname.length !== 0;
    }
}