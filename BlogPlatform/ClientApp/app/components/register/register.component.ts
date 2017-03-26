import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ApplicationRoutes } from '../routes';
import { Registration } from '../../core/domain/registration';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'register',
    template: require('./register.component.html'),
    providers: [MembershipService, DataService, NotificationService]
})

export class RegisterComponent implements OnInit  {
    private applicationRoutes = ApplicationRoutes;
    private router: Router;
    private newUser: Registration;

    constructor(public membershipService: MembershipService,
                public notificationService: NotificationService,
                router: Router) {
        this.newUser = new Registration('', '', '', '', '', '');
        this.router = router;
    }

    ngOnInit() {

    }

    register(): void {
        this.notificationService.printSuccessMessage('Register');

        var registrationResult: OperationResult = new OperationResult(false, '');
        this.membershipService.register(this.newUser)
            .subscribe(res => {
                registrationResult.Succeeded = res["succeeded"];
                registrationResult.Message = res["message"];
            },
            error => console.error('Error: ' + error),
            () => {
                if (registrationResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Dear ' + this.newUser.lastName + ', please login with your credentials');
                    this.router.navigate([this.applicationRoutes.login.path]);
                }
                else {
                    this.notificationService.printErrorMessage(registrationResult.Message);
                }
            });
    };
}