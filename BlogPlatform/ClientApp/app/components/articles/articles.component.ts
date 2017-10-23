import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
import { NotificationService } from '../../core/services/notificationService';
import { OperationResult } from '../../core/domain/operationResult';
import { Constants } from '../../core/constants';

@Component({
    selector: 'articles',
    template: require('./articles.component.html'),
    styles: [require('./articles.component.css')],
    providers: [UtilityService, DataService, NotificationService]
})

export class ArticlesComponent implements OnInit {
    @Input()
    private showSelectedUserArticlesOnly: boolean;

    @Input()
    private canManageArticles: boolean;

    private articlesReadUrl = Constants.BaseUrl + 'api/articles/';
    private articlesDeleteUrl = Constants.BaseUrl + 'api/articles/';
    private articles: Array<Article>;

    constructor(public articlesService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService,
        private activatedRoute: ActivatedRoute) {
        this.articles = [];
        this.showSelectedUserArticlesOnly = false;
        this.canManageArticles = false;
    }

    ngOnInit() {
        if (this.showSelectedUserArticlesOnly) {
            let emailAddress = this.activatedRoute.snapshot.params['emailAddress'];
            this.articlesReadUrl += emailAddress + '/';
        }

        this.getArticles();
    }

    getArticles(): void {
        this.articlesService.set(this.articlesReadUrl);
        this.articlesService.get(1)
            .subscribe(res => {
                var data: any = res.json();
                for (let i = 0; i < data.length; i++) {
                    var account = data[i].account;
                    let author = new BaseProfile(account['firstName'], account['lastName'], account['nickname'], account['emailAddress']);
                    this.articles.push(new Article(
                        data[i]['id'],
                        data[i]['title'],
                        data[i]['content'],
                        data[i]['dateCreated'],
                        data[i]['accountId'],
                        data[i]['totalComments'],
                        data[i]['rating'],
                        author));
                }
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }

    search(articleId: number): void {
        this.getArticles();
    };

    deleteArticle(articleId: number): void {
        this.articlesService.set(this.articlesDeleteUrl);
        let deletArticleResult: OperationResult = new OperationResult(false, '');

        this.notificationService.printConfirmationDialog('Do you want to delete this article?',
            () => {
                this.articlesService.delete(articleId)
                    .subscribe(res => {
                        deletArticleResult.Succeeded = res['succeeded'];
                        deletArticleResult.Message = res['message'];
                    },
                    error => {
                        this.notificationService.printErrorMessage(error);
                    },
                    () => {
                        if (deletArticleResult.Succeeded) {
                            this.notificationService.printSuccessMessage('Article has been deleted');
                        }
                        else {
                            this.notificationService.printErrorMessage(deletArticleResult.Message);
                        }
                        this.refreshArticles();
                    });
            });
    };

    refreshArticles(): void {
        this.articles = [];
        this.getArticles();
    };
}
