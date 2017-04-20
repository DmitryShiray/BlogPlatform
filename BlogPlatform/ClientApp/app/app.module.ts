import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { Location, LocationStrategy, HashLocationStrategy } from '@angular/common';
import { Headers, RequestOptions, BaseRequestOptions } from '@angular/http';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ArticlesComponent } from './components/articles/articles.component';
import { ArticleComponent } from './components/articles/article.component';
import { EditProfileComponent } from './components/profile/editProfile.component';
import { ViewProfileComponent } from './components/profile/viewProfile.component';
import { routing } from './routes';

import { DataService } from './core/services/dataService';
import { MembershipService } from './core/services/membershipService';
import { UtilityService } from './core/services/utilityService';
import { NotificationService } from './core/services/notificationService';
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
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        routing
    ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        ArticlesComponent,
        ArticleComponent
        EditProfileComponent,
        ViewProfileComponent,
        EqualValidator 
    ],
    providers: [DataService, MembershipService, UtilityService, NotificationService,
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: RequestOptions, useClass: AppBaseRequestOptions }],
    bootstrap: [AppComponent]
})
export class AppModule { }

