import { Injectable } from '@angular/core';
import { Http, Response, Request, Headers, RequestOptions } from '@angular/http';
import { OperationResult } from '../../core/domain/operationResult';

let headers = new Headers({ 'Content-Type': 'application/json' });
let options = new RequestOptions({ headers: headers });

import 'rxjs/add/operator/map'

@Injectable()
export class DataService {

    public pageSize: number;
    public baseUri: string;

    constructor(public http: Http) {

    }

    set(baseUri: string, pageSize: number = 10): void {
        this.baseUri = baseUri;
        this.pageSize = pageSize;
    }

    get(page: number) {
        var uri = this.baseUri + page.toString() + '/' + this.pageSize.toString();

        return this.http.get(uri)
            .map(response => (<Response>response));
    }

    getItem(identifier?: any) {
        var uri = this.baseUri + (identifier != null ? identifier.toString() : '/');

        return this.http.get(uri)
            .map(response => (<Response>response));
    }

    post(data?: any, mapJson: boolean = true) {
        if (mapJson)
            return this.http.post(this.baseUri, data, options)
                .map(response => <any>(<Response>response).json());
        else
            return this.http.post(this.baseUri, data);
    }

    delete(identifier: any) {
        var uri = this.baseUri + (identifier != null ? identifier.toString() : '/');

        return this.http.delete(uri)
            .map(response => <any>(<Response>response).json())
    }

    deleteResource(resource: string) {
        return this.http.delete(resource)
            .map(response => <any>(<Response>response).json())
    }
}
