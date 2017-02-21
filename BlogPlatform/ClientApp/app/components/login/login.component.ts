﻿import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AccountRoutes } from '../routes';
import { Account } from '../../core/domain/account';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';


@Component({
    selector: 'login',
    template: require('./login.component.html'),
    providers: [MembershipService, DataService]
})

export class LoginComponent implements OnInit {
    private router: Router;
    private account: Account;
    private accountRoutes = AccountRoutes;

    constructor(public membershipService: MembershipService,
                router: Router) {
        this.account = new Account('', '');
        this.router = router;
        this.accountRoutes = AccountRoutes;
    }

    ngOnInit() {

    }

    login(): void {

        var authenticationResult: OperationResult = new OperationResult(false, '');
        console.log('login');

        this.membershipService.login(this.account)
            .subscribe(res => {
                authenticationResult.Succeeded = res["succeeded"];
                authenticationResult.Message = res["message"];
            },
            error => console.error('Error: ' + error),
            () => {
                if (authenticationResult.Succeeded) {
                    console.log('Welcome back ' + this.account.EmailAddress + '!');
                    
                    this.router.navigate([this.accountRoutes.register.path]);
                }
                else {
                    console.log(authenticationResult.Message);
                    //this.notificationService.printErrorMessage(_authenticationResult.Message);
                }
            });
    };
}