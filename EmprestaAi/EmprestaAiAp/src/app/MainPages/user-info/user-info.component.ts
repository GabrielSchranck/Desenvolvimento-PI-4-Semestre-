import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';

@Component({
  selector: 'app-user-info',
  imports: [RouterModule],
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
