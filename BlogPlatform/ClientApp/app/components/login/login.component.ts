import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AccountRoutes } from '../routes';
import { Account } from '../../core/domain/account';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';


@Component({
    selector: 'login',
    template: require('./login.component.html'),
    providers: [MembershipService, DataService, NotificationService]
})

export class LoginComponent implements OnInit {
    private router: Router;
    private account: Account;
    private accountRoutes = AccountRoutes;

    constructor(public membershipService: MembershipService,
                public notificationService: NotificationService,
                router: Router) {
        this.account = new Account('', '');
        this.router = router;
        this.accountRoutes = AccountRoutes;
    }

    ngOnInit() {

    }

    login(): void {
        this.notificationService.printSuccessMessage('Login started');
        var authenticationResult: OperationResult = new OperationResult(false, '');

        this.membershipService.login(this.account)
            .subscribe(res => {
                authenticationResult.Succeeded = res["succeeded"];
                authenticationResult.Message = res["message"];
            },
            error => this.notificationService.printErrorMessage('Error: ' + error),
            () => {
                if (authenticationResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Welcome back ' + this.account.EmailAddress + '!');
                    this.router.navigateByUrl('/');
                }
                else {
                    this.notificationService.printErrorMessage(authenticationResult.Message);
                }
            });
    };
}