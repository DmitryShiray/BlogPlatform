import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { Rating } from '../../core/domain/rating';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { CommentsComponent } from '../comments/comments.component';
import { BaseComponent } from '../base/baseComponent.component';
import { OperationResult } from '../../core/domain/operationResult';

@Component({
    selector: 'article',
    template: require('./article.component.html'),
    providers: [UtilityService, DataService, NotificationService]
})

export class ArticleComponent extends BaseComponent implements OnInit {
    private articleReadUrl: string = 'api/articles/article/';
    private articleSetRatingUrl: string = 'api/articles/setRating/';
    private article: Article;
    private author: BaseProfile;
    private articleRating: Rating;
    private articleRatingValue: number;

    @ViewChild(CommentsComponent)
    private commentsComponent: CommentsComponent;

    constructor(public articlesService: DataService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                public utilityService: UtilityService,
                private activatedRoute: ActivatedRoute) {
        super(membershipService, notificationService);
        this.author = new BaseProfile('', '', '', '');
        this.article = new Article(0, '', '', null, 0, 0, 0, this.author);
        this.articleRating = new Rating(0, 0, null, 0, this.author);
    }

    ngOnInit() {
        this.getArticle();
    }

    getArticle(): void {
        this.articlesService.set(this.articleReadUrl);

        let articleId = this.activatedRoute.snapshot.params['articleId'];

        this.articlesService.getItem(articleId)
            .subscribe(res => {
                let data: any = res.json();
                let account = data["account"];
                this.author = new BaseProfile(account["firstName"], account["lastName"], account["nickname"], account["emailAddress"]);
                this.articleRatingValue = data["rating"];
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

    refreshComments(): void {
        this.commentsComponent.refreshComments();
    }

    setRating(rating: number): void {
        this.articlesService.set(this.articleSetRatingUrl);

        let articleId: number = this.activatedRoute.snapshot.params['articleId'];
        let addCommentResult: OperationResult = new OperationResult(false, '');
        
        this.articleRating.dateAdded = new Date();
        this.articleRating.value = this.articleRatingValue;
        this.articleRating.articleId = articleId;

        this.articlesService.post(this.articleRating)
            .subscribe(res => {
                addCommentResult.Succeeded = res["succeeded"];
                addCommentResult.Message = res["message"];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (addCommentResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Your rating has been set');
                }
                else {
                    this.notificationService.printErrorMessage(addCommentResult.Message);
                }
            });
    }
}