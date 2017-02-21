import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

export var AccountRoutes = {
    login: { path: '/login', component: LoginComponent},
    register: { path: '/register', component: RegisterComponent }
};

export const ACCOUNT_ROUTES = Object.keys(AccountRoutes).map(r => AccountRoutes[r]);