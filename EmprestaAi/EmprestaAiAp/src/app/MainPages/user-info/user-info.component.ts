import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";

@Component({
  selector: 'app-user-info',
  imports: [RouterModule, NavbarComponent],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css'
})
export class UserInfoComponent {

  constructor(private auth: AuthService, private router: Router) {}

  public logout(): void{
    this.auth.logout();
    this.NavegateToLogin();
  }

  public NavegateToLogin(): void{
    this.router.navigate(['/login']);
  }
}
