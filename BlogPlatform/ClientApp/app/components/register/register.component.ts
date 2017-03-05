import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AccountRoutes } from '../routes';
import { Registration } from '../../core/domain/registration';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
//import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'register',
    template: require('./register.component.html'),
    providers: [MembershipService, DataService]
})

export class RegisterComponent implements OnInit  {

    private accountRoutes = AccountRoutes;
    private router: Router;
    private newUser: Registration;

    constructor(public membershipService: MembershipService,
                router: Router) {
        this.newUser = new Registration('', '', '', '', '', '');
        this.router = router;
        this.accountRoutes = AccountRoutes;
    }

    ngOnInit() {

    }

    register(): void {
        var registrationResult: OperationResult = new OperationResult(false, '');
        this.membershipService.register(this.newUser)
            .subscribe(res => {
                registrationResult.Succeeded = res["succeeded"];
                registrationResult.Message = res["message"];
            },
            error => console.error('Error: ' + error),
            () => {
                if (registrationResult.Succeeded) {
                    console.log('Dear ' + this.newUser.LastName + ', please login with your credentials');
                    this.router.navigate([this.accountRoutes.login.path]);
                }
                else {
                    //this.notificationService.printErrorMessage(_registrationResult.Message);
                    console.log(registrationResult.Message);
                }
            });
    };
}