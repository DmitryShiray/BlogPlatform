import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
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
    //{
    //    path: 'profile',
    //    component: ProfileComponent
    //},
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
