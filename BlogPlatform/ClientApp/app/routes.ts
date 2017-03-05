import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
//import { ProfileComponent } from './components/photos.component';
import { ArticlesComponent } from './components/articles/articles.component';
//import { ArticleCommentsPhotosComponent } from './components/album-photos.component';
import { AccountRoutes, ACCOUNT_ROUTES } from './components/routes';


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
    //{
    //    path: 'articles/:id/comments',
    //    component: ArticleCommentsPhotosComponent
    //}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
