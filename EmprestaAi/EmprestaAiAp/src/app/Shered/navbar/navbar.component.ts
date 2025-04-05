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

  constructor(private auth: AuthService, private router: Router){}

  async ngOnInit(): Promise<void> {
    this.logado = await this.isLoggedIn();
    console.log("Estado = " + this.logado);
  }
  
  private async isLoggedIn(): Promise<boolean> {
    try {
      const logado = await firstValueFrom(this.auth.isLoggedIn());
      console.log(logado);
      return logado;
    } catch (error) {
      console.error("Erro ao verificar login:", error);
      return false;
    }
  }

  logout(): void{
    this.auth.logout();
    this.logado = false;
    this.router.navigate(['/login']);
  }

}
