import { Routes } from '@angular/router';
import { HomeComponent } from './MainPages/home/home.component';
import { LoginComponent } from './Pages/login/login.component';
import { RegisterComponent } from './Pages/register/register.component';
import { PerfilComponent } from './Pages/perfil/perfil.component';
import { LivrosComponent } from './Pages/livros/livros.component';
import { CarrinhoComponent } from './Pages/carrinho/carrinho.component';
import { FavoritosComponent } from './Pages/favoritos/favoritos.component';
import { FinanceiroComponent } from './Pages/financeiro/financeiro.component';
import { GoogleSingInComponent } from './Pages/google-sing-in/google-sing-in.component';
import { ExibeLivroComponent } from './Pages/exibe-livro/exibe-livro.component';


export const routes: Routes = [
    { path: "", component: HomeComponent },
    { path: "carrinho", component: CarrinhoComponent },
    { path: "favoritos", component: FavoritosComponent },
    { path: "financeiro", component: FinanceiroComponent },
    { path: "livros", component: LivrosComponent },
    { path: "login", component: LoginComponent },
    { path: "perfil", component: PerfilComponent },
    { path: "register", component: RegisterComponent },
    { path: "googleSingIn", component: GoogleSingInComponent},
    { path: "exibelivro", component: ExibeLivroComponent}
];
