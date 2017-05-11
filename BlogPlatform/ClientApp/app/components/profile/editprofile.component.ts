import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { isBrowser } from 'angular2-universal';

import { ViewProfileComponent } from './viewProfile.component';

import { ApplicationRoutes } from '../routes';
import { Profile } from '../../core/domain/profile';
import { OperationResult } from '../../core/domain/operationResult';
import { PasswordChange } from '../../core/domain/passwordChange';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { UtilityService } from '../../core/services/utilityService';
import { Constants } from '../../core/constants';

@Component({
    selector: 'editProfile',
    template: require('./editProfile.component.html'),
    providers: [DataService, UtilityService, NotificationService]
})

export class EditProfileComponent extends ViewProfileComponent implements OnInit {
    private profileDeleteUrl: string = 'api/profile/deleteProfile/';
    private profileUpdateUrl: string = 'api/profile/updateProfile/';
    private changePasswordUrl: string = 'api/profile/changePassword/';

    private passwordChange: PasswordChange;

    constructor(public profileService: DataService,
        public notificationService: NotificationService,
        public membershipService: MembershipService,
        public utilityService: UtilityService,
        protected router: Router,
        activatedRoute: ActivatedRoute) {
        super(profileService, notificationService, membershipService, utilityService, router, activatedRoute);
        this.passwordChange = new PasswordChange();
    }

    updateProfile(): void {
        var updateProfileResult: OperationResult = new OperationResult(false, '');

        this.profileService.set(this.profileUpdateUrl);

        this.profileService.post(this.profile)
            .subscribe(res => {
                updateProfileResult.Succeeded = res['succeeded'];
                updateProfileResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (updateProfileResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Profile has been updated');
                }
                else {
                    this.notificationService.printErrorMessage(updateProfileResult.Message);
                }
            });

    }

    changePassword(): void {
        var changePasswordResult: OperationResult = new OperationResult(false, '');

        this.passwordChange.emailAddress = this.profile.emailAddress;
        this.profileService.set(this.changePasswordUrl);

        this.profileService.post(this.passwordChange)
            .subscribe(res => {
                changePasswordResult.Succeeded = res['succeeded'];
                changePasswordResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            },
            () => {
                if (changePasswordResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Your password has been changed');
                }
                else {
                    this.notificationService.printErrorMessage(changePasswordResult.Message);
                }
            });
    }

    deleteProfile(): void {
        this.profileService.set(this.profileDeleteUrl);
        var deletionResult: OperationResult = new OperationResult(false, '');

        this.profileService.delete(this.profile.emailAddress)
            .subscribe(res => {
                deletionResult.Succeeded = res['succeeded'];
                deletionResult.Message = res['message'];
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
                this.utilityService.navigateToSignIn();
            },
            () => {
                if (deletionResult.Succeeded) {
                    this.membershipService.setIsAuthenticated(false);
                    if (isBrowser) {
                        localStorage.removeItem(Constants.EmailAddress);
                    }

                    this.notificationService.printSuccessMessage('Your account has been deleted');
                    this.router.navigate([this.applicationRoutes.home.path]);
                }
                else {
                    this.notificationService.printErrorMessage(deletionResult.Message);
                }
            });
    }
}