import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { isBrowser } from 'angular2-universal';

import { ApplicationRoutes } from '../routes';
import { Profile } from '../../core/domain/profile';
import { OperationResult } from '../../core/domain/operationResult';
import { DataService } from '../../core/services/dataService';
import { MembershipService } from '../../core/services/membershipService';
import { NotificationService } from '../../core/services/notificationService';
import { UtilityService } from '../../core/services/utilityService';
import { Constants } from '../../core/constants';

import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'viewProfile',
    template: require('./viewProfile.component.html'),
    styles: [require('./viewProfile.component.css')],
    providers: [MembershipService, DataService, NotificationService]
})

export class ViewProfileComponent implements OnInit {
    private profileReadUrl: string = 'api/profile/getUserProfile/';

    profile: Profile;
    protected applicationRoutes = ApplicationRoutes;

    constructor(public profileService: DataService,
        public notificationService: NotificationService,
        public membershipService: MembershipService,
        public utilityService: UtilityService,
        protected router: Router,
        private activatedRoute: ActivatedRoute) {
        this.profile = new Profile('','','','', null, false);
    }

    ngOnInit() {
        this.profileService.set(this.profileReadUrl);
        this.getProfile();
    }

    navigateBack(): void {
        if (isBrowser) {
            window.history.back();
        }
    }

    navigateToEditProfile(): void {
        this.router.navigate([this.applicationRoutes.editProfile.path]);
    }
    
    getProfile(): void {
        this.notificationService.printSuccessMessage('Getting profile');

        let emailAddress = this.activatedRoute.snapshot.params['emailAddress'];

        this.profileService.getItem(emailAddress)
            .subscribe(res => {
                var data = res.json();
                this.profile = new Profile(data.firstName, data.lastName, data.nickname, data.emailAddress, data.registrationDate, data.isCurrentUserProfile);
            },
            error => {
                this.notificationService.printErrorMessage('Error ' + error);
            });
    }
}