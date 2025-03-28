import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth-service.service';

@Component({
  selector: 'app-user-info',
  imports: [RouterModule],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css'
})
export class UserInfoComponent {
  router: any;

  constructor(private auth: AuthService){}

  public logout(): void{
    console.log("Entrou aqui")
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
