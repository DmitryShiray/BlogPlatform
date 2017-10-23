import { Http, Response, Request } from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { DataService } from './dataService';
import { Registration } from '../domain/registration';
import { Account } from '../domain/account';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from 'rxjs/Subject';
import { Constants } from '../constants';

@Injectable()
export class MembershipService {
    private accountRegisterAPI = Constants.BaseUrl + 'api/account/register/';
    private accountLoginAPI = Constants.BaseUrl + 'api/account/login/';
    private accountAuthenticationAPI = Constants.BaseUrl + 'api/account/isUserAuthenticated/';
    private accountGetLoggedInUserAPI = Constants.BaseUrl + 'api/account/getLoggedInAccount/';
    private accountLogoutAPI = Constants.BaseUrl + 'api/account/logout/';

    private isAuthenticated = new BehaviorSubject<boolean>(false);
    isAuthenticated$ = this.isAuthenticated.asObservable();
    public setIsAuthenticated(value: boolean) {
        this.isAuthenticated.next(value);
    }

    constructor(@Inject('isBrowser') private isBrowser: boolean,
                public accountService: DataService) {
    }

    register(newUser: Registration) {
        this.accountService.set(this.accountRegisterAPI);
        return this.accountService.post(JSON.stringify(newUser));
    }

    login(creds: Account) {
        this.accountService.set(this.accountLoginAPI);
        return this.accountService.post(JSON.stringify(creds));
    }

    logout() {
        this.accountService.set(this.accountLogoutAPI);
        return this.accountService.post(null, false);
    }

    isUserAuthenticated() {
        this.accountService.set(this.accountAuthenticationAPI);
        return this.accountService.post(null);
    }

    getLoggedInAccount() {
        if (this.isBrowser) {
            return localStorage.getItem(Constants.EmailAddress);
        }

        return null;
    }
}
