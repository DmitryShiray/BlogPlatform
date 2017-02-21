import { Component, OnInit } from '@angular/core';
import { Article } from '../../core/domain/article';
import { DataService } from '../../core/services/dataService';
import { UtilityService } from '../../core/services/utilityService';
//import { NotificationService } from '../../core/services/notificationService';

@Component({
    selector: 'articles',
    template: require('./articles.component.html'),
    providers: [UtilityService, DataService]
})
export class ArticlesComponent implements OnInit {
    private articlesReadUrl: string = 'api/articles/';
    private articles: Array<Article>;

    constructor(public articlesService: DataService,
        public utilityService: UtilityService) {
        
    }

    ngOnInit() {
        this.articlesService.set(this.articlesReadUrl, 3);
        this.getArticles();
    }

    getArticles(): void {
        this.articlesService.get(1)
            .subscribe(res => {
                var data: any = res.json();
                this.articles = data.Items;
            },
            error => {
                if (error.status == 401 || error.status == 404) {
                    //this.notificationService.printErrorMessage('Authentication required');
                    this.utilityService.navigateToSignIn();
                }
            });
    }

    search(articleId): void {
        this.getArticles();
    };

    convertDateTime(date: Date) {
        return this.utilityService.convertDateTime(date);
    }
}