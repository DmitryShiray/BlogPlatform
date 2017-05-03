import { Input, Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseProfile } from '../../core/domain/baseProfile';
import { Comment } from '../../core/domain/comment';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
import { NotificationService } from '../../core/services/notificationService';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: 'comments',
    template: require('./comments.component.html'),
    styles: [require('./comments.component.css')],
    providers: [UtilityService, DataService, NotificationService]
})

export class CommentsComponent implements OnInit {
    private articleId: number;
    private commentsReadUrl: string = 'api/comments/';
    private comments: Array<Comment>;
    private author: BaseProfile;

    constructor(public commentsService: DataService,
        public notificationService: NotificationService,
        public utilityService: UtilityService,
        private activatedRoute: ActivatedRoute,
        private router: Router) {
        this.comments = [];
    }

    ngOnInit() {
        this.articleId = this.activatedRoute.snapshot.params['articleId'];
        this.commentsReadUrl += this.articleId + '/';
        this.commentsService.set(this.commentsReadUrl);
        this.getArticleComments();
    }

    getArticleComments(): void {
        this.commentsService.get(1)
            .subscribe(res => {
                var data: any = res.json();
                for (let i = 0; i < data.length; i++) {
                    let author = data[i].author;
                    this.author = new BaseProfile(author["firstName"], author["lastName"], author["nickname"], author["emailAddress"]);
                    this.comments.push(new Comment(
                        data[i]["id"],
                        data[i]["text"],
                        data[i]["dateAdded"],
                        this.articleId,
                        this.author));
                }
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }

    refreshComments(): void {
        this.comments = [];
        this.getArticleComments();
    }

    onCommentAdded(Comment: Comment) {
        this.refreshComments();
    }
}