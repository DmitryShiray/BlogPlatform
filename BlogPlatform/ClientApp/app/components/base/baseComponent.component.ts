import { Component, OnInit, OnDestroy } from '@angular/core';
import { enableProdMode } from '@angular/core';
import { isBrowser } from 'angular2-universal';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';

enableProdMode();
import { Account } from '../../core/domain/account';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'base-component',
    template: require('./baseComponent.component.html'),
    providers: [DataService, NotificationService]
})

export class BaseComponent implements OnInit, OnDestroy {
    protected isUserAuthenticated: boolean;
    protected subscription: Subscription;
    protected emaiAddress: string;

    constructor(public membershipService: MembershipService,
        public notificationService: NotificationService) {
        this.isUserAuthenticated = false;

        this.subscription = this.membershipService.isAuthenticated$
            .subscribe(value => {
                this.isUserAuthenticated = value;
            });
    }

    ngOnInit() {
        this.checkAuthentication();
    }

    checkAuthentication(): void {
        let userAuthenticationResult: boolean;

        this.membershipService.isUserAuthenticated()
            .subscribe(res => {
                userAuthenticationResult = res['isAuthenticated'];
            },
            error => {
                this.notificationService.printErrorMessage('Error: ' + error);
            },
            () => {
                this.isUserAuthenticated = userAuthenticationResult;
            });
    }

    isUserLoggedIn(): boolean {
        return this.isUserAuthenticated;
    }

    getCurentUserEmailAddress(): string {
        if (this.isUserLoggedIn()) {
            return this.membershipService.getLoggedInAccount();
        }
        else {
            return null;
        }
    }

    ngOnDestroy() {
        // prevent memory leak when component is destroyed
        this.subscription.unsubscribe();
    }
}