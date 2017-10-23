import { Component, Inject, PLATFORM_ID, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';

import { Account } from '../../core/domain/account';
import { BaseComponent } from '../base/baseComponent.component';
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

export class NavMenuComponent extends BaseComponent implements OnInit, OnDestroy {
    constructor( @Inject(PLATFORM_ID) protected platform_id,
        public membershipService: MembershipService,
        public notificationService: NotificationService) {
        super(platform_id, membershipService, notificationService);
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
}
