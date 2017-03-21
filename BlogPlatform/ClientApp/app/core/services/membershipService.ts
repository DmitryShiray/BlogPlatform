﻿import { Http, Response, Request } from '@angular/http';
import { Injectable } from '@angular/core';
import { isBrowser } from 'angular2-universal';
import { DataService } from './dataService';
import { Registration } from '../domain/registration';
import { Account } from '../domain/account';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from 'rxjs/Subject';
import { Constants } from '../../core/constants';

@Injectable()
export class MembershipService {
    private accountRegisterAPI: string = '/api/account/register/';
    private accountLoginAPI: string = '/api/account/login/';
    private accountAuthenticationAPI: string = '/api/account/isUserAuthenticated/';
    private accountGetLoggedInUserAPI: string = '/api/account/getLoggedInAccount/';
    private accountLogoutAPI: string = '/api/account/logout/';

    private isAuthenticated = new BehaviorSubject<boolean>(false);
    isAuthenticated$ = this.isAuthenticated.asObservable();
    public setIsAuthenticated(value: boolean) {
        this.isAuthenticated.next(value);
    }

    constructor(public accountService: DataService) {
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
        if (isBrowser) {
            return localStorage.getItem(Constants.EmailAddress);
        }

        return null;
    }
}