import { Http, Response, Request } from '@angular/http';
import { Injectable } from '@angular/core';
import { DataService } from './dataService';
import { Registration } from '../domain/registration';
import { Account } from '../domain/account';

@Injectable()
export class MembershipService {

    private accountRegisterAPI: string = '/api/account/register/';
    private accountLoginAPI: string = '/api/account/login/';
    private accountLogoutAPI: string = '/api/account/logout/';
    
    constructor(public accountService: DataService) { }

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

    isUserAuthenticated(): boolean {
        return false;
    }

    getLoggedInUser(): Account {
        var account: Account;

        if (this.isUserAuthenticated()) {
            account = new Account('dima@dima.com', '123456');
        }

        return account;
    }
}