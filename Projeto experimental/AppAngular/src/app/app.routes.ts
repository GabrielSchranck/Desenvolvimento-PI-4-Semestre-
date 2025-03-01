import { Routes } from '@angular/router';
import path from 'path/win32';
import { NavbarComponent } from './components/navbar/navbar.component';
import { UseracessComponent } from './components/useracess/useracess.component';
import { LoginComponent } from './pages/login/login.component';
import { RegistroComponent } from './pages/registro/registro.component';
import { UserinfoComponent } from './components/userinfo/userinfo.component';

export const routes: Routes = [
    {path: "", component: NavbarComponent},
    {path: "login", component: LoginComponent},
    {path: "registrar", component: RegistroComponent},
    {path: "user", component: UserinfoComponent}
];
