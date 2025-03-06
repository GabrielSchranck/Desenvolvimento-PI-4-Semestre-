import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterOutlet, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{

  logado: boolean = false;

  constructor(private auth: AuthService, private router: Router){}

  ngOnInit(): void {
    this.logado = this.isLoggedIn();
  }

  isLoggedIn(): boolean{
    return this.auth.isLoggedIn();
  }

  logout(): void {
    this.auth.logout();
    this.logado = false;
    this.router.navigate(['/login']);
  }

}
