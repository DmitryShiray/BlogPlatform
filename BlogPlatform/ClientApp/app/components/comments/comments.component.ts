import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { BaseProfile } from '../../core/domain/baseProfile';
import { Comment } from '../../core/domain/comment';
import { Constants } from '../../core/constants';
import { DataService } from '../../core/services/dataService';
import { NotificationService } from '../../core/services/notificationService';
import { UtilityService } from '../../core/services/utilityService';

@Component({
    selector: 'comments',
    template: require('./comments.component.html'),
    styles: [require('./comments.component.css')],
    providers: [UtilityService, DataService, NotificationService]
})

export class CommentsComponent implements OnInit {
    private articleId: number;
    private commentsReadUrl = Constants.BaseUrl + 'api/comments/';
    private comments: Array<Comment>;
    private author: BaseProfile;
    private isGettingCommentsInProgress: boolean;

    constructor(public commentsService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService,
        private activatedRoute: ActivatedRoute,
        private router: Router) {
        this.comments = [];
        this.isGettingCommentsInProgress = false;
    }

    ngOnInit() {
        this.articleId = this.activatedRoute.snapshot.params['articleId'];
        this.commentsReadUrl += this.articleId + '/';
        this.commentsService.set(this.commentsReadUrl);
        this.getArticleComments();
    }

    getArticleComments(): void {
        this.isGettingCommentsInProgress = true;

        this.commentsService.get(1)
            .subscribe(res => {
                var data: any = res.json();
                for (let i = 0; i < data.length; i++) {
                    let author = data[i].author;
                    this.author = new BaseProfile(author['firstName'], author['lastName'], author['nickname'], author['emailAddress']);
                    this.comments.push(new Comment(
                        data[i]['id'],
                        data[i]['text'],
                        data[i]['dateAdded'],
                        this.articleId,
                        this.author));
                }
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                this.isGettingCommentsInProgress = false;
            });
    }

    refreshComments(): void {
        this.comments = [];
        if (!this.isGettingCommentsInProgress) {
            this.getArticleComments();
        }
    }

    onCommentAdded(Comment: Comment) {
        this.refreshComments();
    }
}
