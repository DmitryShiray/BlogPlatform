import { Input, Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { BaseProfile } from '../../core/domain/baseProfile';
import { Article } from '../../core/domain/article';
import { UtilityService } from '../../core/services/utilityService';

@Component({
    selector: 'author',
    template: require('./author.component.html'),
    styles: [require('./author.component.css')],
    providers: [UtilityService]
})

export class AuthorComponent {
    @Input() author: BaseProfile;
    @Input() dateCreated: Date;
    @Input() rating: number;

    constructor(public utilityService: UtilityService) {
    }

    public convertDateTime() {
        return this.utilityService.convertDateTimeToString(this.dateCreated);
    }

    public showNickname(): boolean {
        return this.author.nickname && this.author.nickname.length !== 0;
    }
}