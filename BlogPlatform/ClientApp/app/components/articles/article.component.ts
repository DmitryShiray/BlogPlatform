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
    private author: BaseProfile;

    constructor(public articlesService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService,
        private activatedRoute: ActivatedRoute) {
        this.article = new Article(0, '', '', null,0, 0, 0, null);
        this.author = new BaseProfile('', '', '', '');
    }

    ngOnInit() {
        this.articlesService.set(this.articlesReadUrl);
        this.getArticle();
    }

    getArticle(): void {
        let articleId = this.activatedRoute.snapshot.params['articleId'];

        this.articlesService.getItem(articleId)
            .subscribe(res => {
                this.notificationService.printSuccessMessage('Test');

                let data: any = res.json();
                let account = data["account"];
                this.author = new BaseProfile(account["firstName"], account["lastName"], account["nickname"], account["emailAddress"]);
                this.article = new Article(
                    data["id"],
                    data["title"],
                    data["content"],
                    data["dateCreated"],
                    data["accountId"],
                    data["totalComments"],
                    data["rating"],
                    this.author);
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }
}