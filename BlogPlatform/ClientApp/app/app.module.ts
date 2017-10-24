import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Headers, HttpModule, JsonpModule, RequestOptions, BaseRequestOptions } from '@angular/http';
import { HashLocationStrategy, Location, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CKEditorModule } from 'ng2-ckeditor';

import { AddCommentsComponent } from './components/comments/addComments.component';
import { AppComponent } from './components/app/app.component';
import { ArticlesComponent } from './components/articles/articles.component';
import { ArticleComponent } from './components/articles/article.component';
import { AuthorComponent } from './components/author/author.component';
import { BaseComponent } from './components/base/baseComponent.component';
import { CommentsComponent } from './components/comments/comments.component';
import { EditProfileComponent } from './components/profile/editProfile.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ManageArticleComponent } from './components/articles/manageArticle.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { RatingComponent } from './components/rating/rating.component';
import { RegisterComponent } from './components/register/register.component';
import { ViewProfileComponent } from './components/profile/viewProfile.component';

import { routing } from './routes';

import { DataService } from './core/services/dataService';
import { EqualValidator } from './directives/equalValidator.directive';
import { MembershipService } from './core/services/membershipService';
import { NotificationService } from './core/services/notificationService';
import { SignalRService } from './core/services/signalRService';
import { UtilityService } from './core/services/utilityService';

class AppBaseRequestOptions extends BaseRequestOptions {
    headers: Headers = new Headers();

    constructor() {
        super();
        this.headers.append('Content-Type', 'application/json');
        this.body = '';
    }
}

@NgModule({
    imports: [
        BrowserModule,
        CKEditorModule,
        FormsModule,
        HttpModule,
        JsonpModule,
        routing
    ],
    declarations: [
        AddCommentsComponent,
        AppComponent,
        ArticlesComponent,
        ArticleComponent,
        AuthorComponent,
        BaseComponent,
        CommentsComponent,
        EditProfileComponent,
        EqualValidator,
        HomeComponent,
        LoginComponent,
        ManageArticleComponent,
        NavMenuComponent,
        RegisterComponent,
        RatingComponent,
        ViewProfileComponent
    ],
    providers: [
        DataService,
        MembershipService,
        NotificationService,
        SignalRService,
        UtilityService,
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: RequestOptions, useClass: AppBaseRequestOptions },
        { provide: 'isBrowser', useValue: true }
    ],
    bootstrap: [AppComponent]
})
    
export class AppModule { }

