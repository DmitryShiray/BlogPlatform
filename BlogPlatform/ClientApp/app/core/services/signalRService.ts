import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

import { Constants } from '../constants';

@Injectable()
export class SignalRService {
    private hubConnection: HubConnection;
    public commentAdded = new EventEmitter();

    constructor() {
        this.hubConnection = new HubConnection(Constants.BaseUrl + 'commentsHub');
        this.registerOnServerEvents();
        this.startConnection();
    }

    private startConnection(): void {
        this.hubConnection.start();
    }

    private registerOnServerEvents(): void {
        this.hubConnection.on('CommentAdded', (data: any) => {
            console.log('CommentAdded ' + data);
            this.commentAdded.emit();
        });
    }
}
