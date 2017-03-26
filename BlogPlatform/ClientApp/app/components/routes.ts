import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ArticlesComponent } from './articles/articles.component';
import { HomeComponent } from './home/home.component';
import { ViewProfileComponent } from './profile/viewProfile.component';
import { EditProfileComponent } from './profile/editProfile.component';

export var ApplicationRoutes = {
    login: { path: '/login', component: LoginComponent},
    register: { path: '/register', component: RegisterComponent },
    articles: { path: '/articles', component: ArticlesComponent },
    home: { path: '/home', component: HomeComponent },
    viewProfile: { path: '/viewProfile', component: ViewProfileComponent },
    editProfile: { path: '/editProfile', component: EditProfileComponent }
}

export const APPLICATION_ROUTES = Object.keys(ApplicationRoutes).map(r => ApplicationRoutes[r]);