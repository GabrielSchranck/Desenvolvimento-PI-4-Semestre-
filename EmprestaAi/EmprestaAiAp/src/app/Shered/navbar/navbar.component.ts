import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';
import { firstValueFrom, Observable } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  logado: boolean = false;
  nomeCliente: string = "Nome do cliente";

  constructor(private auth: AuthService, private router: Router){}

  async ngOnInit(): Promise<void> {
    this.logado = await this.isLoggedIn();
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

}
