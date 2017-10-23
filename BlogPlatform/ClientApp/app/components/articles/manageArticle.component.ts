import { Component, Input, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { ApplicationRoutes } from '../routes';
import { BaseArticleComponent } from './baseArticle.component';
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
import { Constants } from '../../core/constants';

@Component({
    selector: 'manageArticle',
    template: require('./manageArticle.component.html'),
    styles: [require('./manageArticle.component.css')]
})

export class ManageArticleComponent extends BaseArticleComponent implements OnInit {
    @Input()
    private editMode: boolean;

    private applicationRoutes = ApplicationRoutes;

    private articleCreateUrl = Constants.BaseUrl + 'api/articles/create/';
    private articleUpdateUrl = Constants.BaseUrl + 'api/articles/update/';

    //used for validation due to angular form binding error caused by ckeditor.
    private saveButtonClicked: boolean;

    private articleId: number;

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public articlesService: DataService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                public utilityService: UtilityService,
                activatedRoute: ActivatedRoute,
                private router: Router) {
        super(platform_id, articlesService, membershipService, notificationService, activatedRoute);
        let author = new BaseProfile('', '', '', '');
        this.article = new Article(0, '', '', null, 0, 0, 0, author);
        this.articleId = 0;

        this.saveButtonClicked = false;
    }

    ngOnInit() {
        super.ngOnInit();
        this.editMode = this.activatedRoute.snapshot.data['editMode'];

        if (this.editMode) {
            this.articleId = +this.activatedRoute.snapshot.params['articleId'];
            this.getArticle();
            this.articlesService.set(this.articleUpdateUrl);
        }
        else {
            this.articlesService.set(this.articleCreateUrl);
        }
    }

    saveArticle(): void {
        if (this.article.title == null
            || this.article.content == null
            || this.article.title.length === 0
            || this.article.content.length === 0) {
            this.saveButtonClicked = true;
            return;
        }

        let articleCreationResult: OperationResult = new OperationResult(false, '');

        this.article.id = this.articleId;
        this.article.dateCreated = new Date();

        this.articlesService.post(this.article)
            .subscribe(res => {
                articleCreationResult.Succeeded = res['succeeded'];
                articleCreationResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (articleCreationResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Your article has been saved');
                    this.router.navigate([this.applicationRoutes.articles.path]);
                }
                else {
                    this.notificationService.printErrorMessage(articleCreationResult.Message);
                }
            });
    }
}
