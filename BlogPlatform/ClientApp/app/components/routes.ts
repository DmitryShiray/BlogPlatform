import { ArticleComponent } from './articles/article.component';
import { ArticlesComponent } from './articles/articles.component';
import { EditProfileComponent } from './profile/editProfile.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ViewProfileComponent } from './profile/viewProfile.component';

export var ApplicationRoutes = {
    article: { path: '/articles/article/:articleId', component: ArticleComponent },
    articles: { path: '/articles', component: ArticlesComponent },
    editProfile: { path: '/editProfile', component: EditProfileComponent },
    home: { path: '/home', component: HomeComponent },
    login: { path: '/login', component: LoginComponent},
    register: { path: '/register', component: RegisterComponent },
    viewProfile: { path: '/viewProfile/:emailAddress', component: ViewProfileComponent }
}

export const APPLICATION_ROUTES = Object.keys(ApplicationRoutes).map(r => ApplicationRoutes[r]);
