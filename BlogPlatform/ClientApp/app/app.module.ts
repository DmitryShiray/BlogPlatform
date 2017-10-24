import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { Location, LocationStrategy, HashLocationStrategy } from '@angular/common';
import { Headers, RequestOptions, BaseRequestOptions } from '@angular/http';

import { CKEditorModule } from 'ng2-ckeditor';

import { AppComponent } from './components/app/app.component';
import { BaseComponent } from './components/base/baseComponent.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthorComponent } from './components/author/author.component';
import { ArticlesComponent } from './components/articles/articles.component';
import { ArticleComponent } from './components/articles/article.component';
import { ManageArticleComponent } from './components/articles/manageArticle.component';
import { CommentsComponent } from './components/comments/comments.component';
import { AddCommentsComponent } from './components/comments/addComments.component';
import { RatingComponent } from './components/rating/rating.component';
import { EditProfileComponent } from './components/profile/editProfile.component';
import { ViewProfileComponent } from './components/profile/viewProfile.component';
import { routing } from './routes';

import { DataService } from './core/services/dataService';
import { MembershipService } from './core/services/membershipService';
import { UtilityService } from './core/services/utilityService';
import { NotificationService } from './core/services/notificationService';
import { SignalRService } from './core/services/signalRService';
import { EqualValidator } from './directives/equalValidator.directive';

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
        FormsModule,
        CKEditorModule,
        HttpModule,
        JsonpModule,
        routing
    ],
    declarations: [
        AppComponent,
        BaseComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        AuthorComponent,
        ArticlesComponent,
        ArticleComponent,
        ManageArticleComponent,
        CommentsComponent,
        AddCommentsComponent,
        RatingComponent,
        EditProfileComponent,
        ViewProfileComponent,
        EqualValidator 
    ],
    providers: [
        DataService,
        MembershipService,
        UtilityService,
        NotificationService,
        SignalRService,
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: RequestOptions, useClass: AppBaseRequestOptions },
        { provide: 'isBrowser', useValue: true }
    ],
    bootstrap: [AppComponent]
})
    
export class AppModule { }

