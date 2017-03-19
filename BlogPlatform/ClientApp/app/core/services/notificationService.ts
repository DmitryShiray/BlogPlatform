import { Injectable } from '@angular/core';
import { isBrowser } from 'angular2-universal';

@Injectable()
export class NotificationService {
    private notifier: any;

    constructor() {
        if (isBrowser) {
            this.notifier = require('alertify.js');

            this.notifier.delay(3000);
            this.notifier.logPosition("bottom right");
        }
    }

    printSuccessMessage(message: string) {
        this.notifier.success(message);
    }

    printErrorMessage(message: string) {
        this.notifier.error(message);
    }

    printConfirmationDialog(message: string, okCallback: () => any) {
        this.notifier.confirm(message, function (e) {
            if (e) {
                okCallback();
            } else {

            }
        });
    }
}