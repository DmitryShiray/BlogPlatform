import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ArticlesComponent } from './articles/articles.component';

export var ApplicationRoutes = {
    login: { path: '/login', component: LoginComponent},
    register: { path: '/register', component: RegisterComponent },
    articles: { path: '/articles', component: ArticlesComponent }
};

export const APPLICATION_ROUTES = Object.keys(ApplicationRoutes).map(r => ApplicationRoutes[r]);