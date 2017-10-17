import { Component, Output, EventEmitter, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseProfile } from '../../core/domain/baseProfile';
import { Comment } from '../../core/domain/comment';
import { DataService } from '../../core/services/dataService';
import { BaseComponent } from '../base/baseComponent.component';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { OperationResult } from '../../core/domain/operationResult';

@Component({
    selector: 'addComments',
    template: require('./addComments.component.html'),
    styles: [require('./addComments.component.css')],
    providers: [DataService, NotificationService]
})

export class AddCommentsComponent extends BaseComponent implements OnInit {
    private authorEmailAddress: string;
    private comment: Comment;

    private commentsAddUrl: string = 'api/comments/addComment';
    private commentText: string;

    @Output()
    public onCommentAdded = new EventEmitter<Comment>();

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public commentsService: DataService,
                public notificationService: NotificationService,
                public membershipService: MembershipService,
                private activatedRoute: ActivatedRoute,
                private router: Router) {
        super(platform_id, membershipService, notificationService);
        let author = new BaseProfile('', '', '', '');
        this.comment = new Comment(0, '', null, 0, author);
    }

    ngOnInit() {
        super.ngOnInit();
        this.comment.articleId = +this.activatedRoute.snapshot.params['articleId'];
        this.commentsService.set(this.commentsAddUrl);
    }

    addComment(): void {
        let addCommentResult: OperationResult = new OperationResult(false, '');

        this.comment.dateAdded = new Date();
        this.comment.text = this.commentText;


        this.commentsService.post(this.comment)
            .subscribe(res => {
                addCommentResult.Succeeded = res['succeeded'];
                addCommentResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (addCommentResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Your comment has been added');
                    this.onCommentAdded.emit(this.comment);
                    this.comment.text = '';
                }
                else {
                    this.notificationService.printErrorMessage(addCommentResult.Message);
                }
            });
    }
}