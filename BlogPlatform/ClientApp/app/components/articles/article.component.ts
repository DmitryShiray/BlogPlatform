import { Component, OnInit, ViewChild, Inject, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { BaseArticleComponent } from './baseArticle.component';
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

export class ArticleComponent extends BaseArticleComponent implements OnInit {
    private articleSetRatingUrl: string = 'api/articles/setRating/';
    private articleRating: Rating;

    @ViewChild(CommentsComponent)
    private commentsComponent: CommentsComponent;

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public articlesService: DataService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                public utilityService: UtilityService,
                activatedRoute: ActivatedRoute) {
        super(platform_id, articlesService, membershipService, notificationService, activatedRoute);
        this.author = new BaseProfile('', '', '', '');
        this.article = new Article(0, '', '', null, 0, 0, 0, this.author);
        this.articleRating = new Rating(0, 0, null, 0, this.author);
    }

    ngOnInit() {
        super.ngOnInit();
        this.getArticle();
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
                addCommentResult.Succeeded = res['succeeded'];
                addCommentResult.Message = res['message'];
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