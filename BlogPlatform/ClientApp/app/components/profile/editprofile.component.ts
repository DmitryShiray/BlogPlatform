import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { ApplicationRoutes } from '../routes';
import { Constants } from '../../core/constants';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { OperationResult } from '../../core/domain/operationResult';
import { Profile } from '../../core/domain/profile';
import { PasswordChange } from '../../core/domain/passwordChange';
import { UtilityService } from '../../core/services/utilityService';

import { ViewProfileComponent } from './viewProfile.component';

@Component({
    selector: 'editProfile',
    template: require('./editProfile.component.html'),
    providers: [DataService, UtilityService, NotificationService]
})

export class EditProfileComponent extends ViewProfileComponent implements OnInit {
    private currentProfileGetUrl = Constants.BaseUrl + 'api/profile/getCurrentUserProfile/';
    private profileDeleteUrl = Constants.BaseUrl + 'api/profile/deleteProfile/';
    private profileUpdateUrl = Constants.BaseUrl + 'api/profile/updateProfile/';
    private changePasswordUrl = Constants.BaseUrl + 'api/profile/changePassword/';

    private passwordChange: PasswordChange;

    constructor(@Inject(PLATFORM_ID) protected platform_id,
                public profileService: DataService,
                public notificationService: NotificationService,
                public membershipService: MembershipService,
                public utilityService: UtilityService,
                protected router: Router,
                activatedRoute: ActivatedRoute) {
        super(platform_id, profileService, notificationService, membershipService, utilityService, router, activatedRoute);
        this.passwordChange = new PasswordChange();
    }

    ngOnInit() {
        this.getProfile();
    }

    getProfile(): void {
        this.profileService.set(this.currentProfileGetUrl);

        this.profileService.getItem()
            .subscribe(res => {
                var data = res.json();
                this.profile = new Profile(data.firstName, data.lastName, data.nickname, data.emailAddress, data.registrationDate, data.isCurrentUserProfile);
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
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
                    if (this.isBrowser) {
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
