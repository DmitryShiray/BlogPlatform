import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class UtilityService {

    private _router: Router;

    constructor(router: Router) {
        this._router = router;
    }

    convertFromDateTimeString(date: string) {
        return this.convertDateTimeToString(new Date(date));
    }

    convertDateTimeToString(date: Date) {
        return new Date(date).toDateString();
    }

    navigate(path: string) {
        this._router.navigate([path]);
    }

    navigateToSignIn() {
        this.navigate('/login');
    }
}
