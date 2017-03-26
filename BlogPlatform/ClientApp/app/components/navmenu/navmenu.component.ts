import { Component, OnInit, OnDestroy } from '@angular/core';
import { enableProdMode } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';

enableProdMode();
import { Account } from '../../core/domain/account';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')],
    providers: [DataService, NotificationService]
})

export class NavMenuComponent implements OnInit, OnDestroy {
    private isUserAuthenticated: boolean;
    private subscription: Subscription;
    private emaiAddress: string;

    constructor(public membershipService: MembershipService,
        public notificationService: NotificationService) {
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

    getEmailAddress(): string {
        if (this.isUserLoggedIn()) {
            return this.membershipService.getLoggedInAccount();
        }
        else {
            return null;
        }
    }

    logout(): void {
        this.membershipService.logout()
            .subscribe(res => {
                this.notificationService.printSuccessMessage('Logout');
            },
            error => {
                this.notificationService.printErrorMessage('Error: ' + error)
            },
            () => {
                this.membershipService.setIsAuthenticated(false);
                this.isUserAuthenticated = false;
            });
    }

    ngOnDestroy() {
        // prevent memory leak when component is destroyed
        this.subscription.unsubscribe();
    }
}