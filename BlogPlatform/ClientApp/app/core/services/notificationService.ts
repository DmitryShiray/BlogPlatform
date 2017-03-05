﻿import { Injectable } from '@angular/core';

@Injectable()
export class NotificationService {
    private notifier: any;

    constructor() {
        this.notifier = require('alertify.js');
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