import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApplicationRoutes } from '../routes';
import { Account } from '../../core/domain/account';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { Constants } from '../../core/constants';

@Component({
    selector: 'login',
    template: require('./login.component.html'),
    providers: [DataService, NotificationService]
})

export class LoginComponent implements OnInit {
    private router: Router;
    private account: Account;
    private applicationRoutes = ApplicationRoutes;

    constructor(@Inject('isBrowser') private isBrowser: boolean,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                router: Router) {
        this.account = new Account('', '');
        this.router = router;
    }

    ngOnInit() {
    }
    
    login(): void {
        var authenticationResult: OperationResult = new OperationResult(false, '');

        this.membershipService.login(this.account)
            .subscribe(res => {
                authenticationResult.Succeeded = res['succeeded'];
                authenticationResult.Message = res['message'];
            },
            error => this.notificationService.printErrorMessage('Error: ' + error),
            () => {
                this.membershipService.setIsAuthenticated(authenticationResult.Succeeded);

                if (authenticationResult.Succeeded) {
                    if (this.isBrowser) {
                        localStorage.setItem(Constants.EmailAddress, this.account.emailAddress);
                    }

                    this.notificationService.printSuccessMessage('Welcome back ' + this.account.emailAddress + '!');
                    this.router.navigate([this.applicationRoutes.articles.path]);
                }
                else {
                    this.notificationService.printErrorMessage(authenticationResult.Message);
                }
            });
    }
}