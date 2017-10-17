import { Injectable, Inject } from '@angular/core';

@Injectable()
export class NotificationService {
    private notifier: any;

    constructor(@Inject('isBrowser') private isBrowser: boolean) {
        if (isBrowser) {
            this.notifier = require('alertify.js');

            this.notifier.delay(3000);
            this.notifier.logPosition('bottom right');
        }
    }

    printSuccessMessage(message: string) {
        this.notifier.success(message);
    }

    printErrorMessage(message: string) {
        this.notifier.error(message);
    }

    printConfirmationDialog(message: string, okCallback: () => any) {
        this.notifier.confirm(message,
            function (e) {
                if (e) {
                    okCallback();
                } else {

                }
            });
    }
}