import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { Article } from '../../core/domain/article';
import { BaseComponent } from '../base/baseComponent.component';
import { BaseProfile } from '../../core/domain/baseProfile';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { OperationResult } from '../../core/domain/operationResult';

@Component({
    selector: 'baseArticle',
    template: require('./baseArticle.component.html'),
    providers: [DataService, NotificationService]
})

export class BaseArticleComponent extends BaseComponent {
    protected articleReadUrl: string = 'api/articles/article/';
    protected article: Article;
    protected author: BaseProfile;
    protected articleRatingValue: number;

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public articlesService: DataService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                protected activatedRoute: ActivatedRoute) {
        super(platform_id, membershipService, notificationService);
        this.author = new BaseProfile('', '', '', '');
        this.article = new Article(0, '', '', null, 0, 0, 0, this.author);
    }

    getArticle(): void {
        this.articlesService.set(this.articleReadUrl);

        let articleId = this.activatedRoute.snapshot.params['articleId'];

        this.articlesService.getItem(articleId)
            .subscribe(res => {
                let data: any = res.json();
                let account = data['account'];
                this.author = new BaseProfile(account['firstName'], account['lastName'], account['nickname'], account['emailAddress']);
                this.articleRatingValue = data['rating'];
                this.article = new Article(
                    data['id'],
                    data['title'],
                    data['content'],
                    data['dateCreated'],
                    data['accountId'],
                    data['totalComments'],
                    data['rating'],
                    this.author);
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }
}