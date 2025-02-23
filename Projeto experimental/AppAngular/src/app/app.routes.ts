import { Routes } from '@angular/router';
import path from 'path/win32';
import { ClienteComponent } from './components/cliente/cliente.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './pages/login/login.component';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {path: "perfil-cliente", component: ClienteComponent},
    {path: "login", component: LoginComponent}
];
