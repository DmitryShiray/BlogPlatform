import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AddCommentsComponent} from './components/comments/addComments.component';
import { ArticleComponent } from './components/articles/article.component';
import { ArticlesComponent } from './components/articles/articles.component';
import { CommentsComponent } from './components/comments/comments.component';
import { EditProfileComponent } from './components/profile/editProfile.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ManageArticleComponent } from './components/articles/manageArticle.component';
import { RegisterComponent } from './components/register/register.component';
import { ViewProfileComponent } from './components/profile/viewProfile.component';

import { ApplicationRoutes, APPLICATION_ROUTES } from './components/routes';

const appRoutes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'articles',
        component: ArticlesComponent
    },
    {
        path: 'articles/article/:articleId',
        component: ArticleComponent
    },
    {
        path: 'editProfile',
        component: EditProfileComponent
    },
    {
        path: 'viewProfile/:emailAddress',
        component: ViewProfileComponent
    },
    {
        path: 'comments',
        component: CommentsComponent
    },
    {
        path: 'addComments',
        component: AddCommentsComponent
    },
    {
        path: 'manageArticle/create',
        component: ManageArticleComponent,
        data: { editMode: false }
    },
    {
        path: 'manageArticle/edit/:articleId',
        component: ManageArticleComponent,
        data: { editMode: true }
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
