import {Routes} from "@angular/router";
import {AppComponent} from "./app.component";
import {LoginComponent} from "../login/login.component";

export const appRoutes: Routes = [
    {
        path: 'login', 
        component: LoginComponent
    },
    {
        path: '',
        component: AppComponent
    }
];

export const appComponents = [
    LoginComponent
];