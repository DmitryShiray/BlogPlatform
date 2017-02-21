import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import { enableProdMode } from '@angular/core';

enableProdMode();
import { MembershipService } from '../../core/services/membershipService';
import { Account } from '../../core/domain/account';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')]
})

export class NavMenuComponent implements OnInit {
    constructor(public membershipService: MembershipService,
        public location: Location) { }

    ngOnInit() { }

    isUserLoggedIn(): boolean {
        return this.membershipService.isUserAuthenticated();
    }

    getEmailAddress(): string {
        if (this.isUserLoggedIn()) {
            var account = this.membershipService.getLoggedInUser();
            return account.EmailAddress;
        }
        else {
            return '';
        }
    }

    logout(): void {
        this.membershipService.logout()
            .subscribe(res => {
                console.log('logout');
            },
            error => console.error('Error: ' + error),
            () => { });
    }
}