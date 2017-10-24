import { Component, Input, } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { Article } from '../../core/domain/article';
import { BaseProfile } from '../../core/domain/baseProfile';
import { UtilityService } from '../../core/services/utilityService';

@Component({
    selector: 'author',
    template: require('./author.component.html'),
    providers: [UtilityService]
})

export class AuthorComponent {
    @Input() author: BaseProfile;
    @Input() dateCreated: Date;
    @Input() rating?: number;

    constructor(public utilityService: UtilityService) {
    }

    public convertDateTime() {
        return this.utilityService.convertDateTimeToString(this.dateCreated);
    }

    public showNickname(): boolean {
        return this.author.nickname && this.author.nickname.length !== 0;
    }

    public showRating(): boolean {
        return this.rating != null;
    }
}
