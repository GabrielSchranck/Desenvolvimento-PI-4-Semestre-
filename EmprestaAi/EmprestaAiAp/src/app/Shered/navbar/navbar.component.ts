import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  logado: boolean = false;

  constructor(private auth: AuthService, private router: Router){}

  ngOnInit(): void {
    this.logado = this.isLoggedIn();
  }

  isLoggedIn(): boolean{
    return this.auth.isLoggedIn();
  }

  logout(): void{
    this.auth.logout();
    this.logado = false;
    this.router.navigate(['/login']);
  }

}
