import { Component, Inject, OnInit, PLATFORM_ID, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { Article } from '../../core/domain/article';
import { BaseArticleComponent } from './baseArticle.component';
import { BaseComponent } from '../base/baseComponent.component';
import { BaseProfile } from '../../core/domain/baseProfile';
import { CommentsComponent } from '../comments/comments.component';
import { Constants } from '../../core/constants';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { OperationResult } from '../../core/domain/operationResult';
import { Rating } from '../../core/domain/rating';
import { SignalRService } from '../../core/services/signalRService';
import { UtilityService } from '../../core/services/utilityService';

@Component({
    selector: 'article',
    template: require('./article.component.html'),
    providers: [UtilityService, DataService, NotificationService]
})

export class ArticleComponent extends BaseArticleComponent implements OnInit {
    private articleSetRatingUrl = Constants.BaseUrl + 'api/articles/setRating/';
    private articleRating: Rating;

    @ViewChild(CommentsComponent)
    private commentsComponent: CommentsComponent;

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public articlesService: DataService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                public utilityService: UtilityService,
                private signalRService: SignalRService,
                activatedRoute: ActivatedRoute) {
        super(platform_id, articlesService, membershipService, notificationService, activatedRoute);
        this.author = new BaseProfile('', '', '', '');
        this.article = new Article(0, '', '', null, 0, 0, 0, this.author);
        this.articleRating = new Rating(0, 0, null, 0, this.author);
    }

    ngOnInit() {
        super.ngOnInit();
        this.getArticle();
        this.subscribeToCommentAddedEvent();
    }

    refreshComments(): void {
        this.commentsComponent.refreshComments();
    }

    setRating(rating: number): void {
        this.articlesService.set(this.articleSetRatingUrl);

        let articleId: number = this.activatedRoute.snapshot.params['articleId'];
        let setRatingResult: OperationResult = new OperationResult(false, '');
        
        this.articleRating.dateAdded = new Date();
        this.articleRating.value = this.articleRatingValue;
        this.articleRating.articleId = articleId;

        this.articlesService.post(this.articleRating)
            .subscribe(res => {
                setRatingResult.Succeeded = res['succeeded'];
                setRatingResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (setRatingResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Your rating has been set');
                }
                else {
                    this.notificationService.printErrorMessage(setRatingResult.Message);
                }
            });
    }

    private subscribeToCommentAddedEvent(): void {
        this.signalRService.commentAdded.subscribe(() => {
            this.article.totalComments += 1;
        });
    }
}
