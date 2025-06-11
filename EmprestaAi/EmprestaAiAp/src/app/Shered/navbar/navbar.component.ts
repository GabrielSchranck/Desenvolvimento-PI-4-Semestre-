import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';
import { firstValueFrom, Observable } from 'rxjs';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  logado: boolean = false;
  nomeCliente: string = ""

  constructor(private auth: AuthService, private router: Router, private clienteService: ClienteService){}

  async ngOnInit(): Promise<void> {
    this.logado = await this.isLoggedIn();

    if(this.logado){
      await this.getClienteInfos();
    }
  }
  
  public isPerfilRota(): boolean {
    const rotasPerfil = ['/perfil', '/carrinho', '/favoritos', '/financeiro', '/livros'];
    return rotasPerfil.includes(this.router.url);
  }

 public isVendaRota(): boolean {
  const rotasVenda = ['', '/'];
  const urlAtual = this.router.url;

  return (
    rotasVenda.includes(urlAtual) ||
    urlAtual.startsWith('/exibelivro/')
  );
}


  private async isLoggedIn(): Promise<boolean> {
    try {
      const logado = await firstValueFrom(this.auth.isLoggedIn());
      return logado;
    } catch (error) {
      return false;
    }
  }

  logout(): void{
    this.auth.logout();
    this.logado = false;
    this.router.navigate(['/login']);
  }

  private getClienteInfos(): void {
  this.clienteService.GetByToken().subscribe({
    next: (dados) => {
      const nomeCompleto: string = dados.cliente.nome?.trim() || '';
      const partes = nomeCompleto.split(' ').filter(p => p.length > 0);

      if (partes.length > 1) {
        this.nomeCliente = `${partes[0]} ${partes[partes.length - 1]}`;
      } else {
        this.nomeCliente = partes[0] || '';
      }
    },
    error: (error) => {
      console.error("Erro ao selecionar clientes:", error);
    }
  });
}


}
