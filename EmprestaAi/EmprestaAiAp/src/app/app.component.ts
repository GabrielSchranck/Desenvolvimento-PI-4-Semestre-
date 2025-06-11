import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { initFlowbite } from 'flowbite';
import { NotificationService } from './core/services/notification.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from './core/services/auth-service.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  constructor(
    private notificationService: NotificationService,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) {}


  ngOnInit(): void {
    initFlowbite();

    this.authService.getClienteId().subscribe({
      next: userId => {
        this.notificationService.startConnection(userId.toString());

        this.notificationService.notification$.subscribe(msg => {
          if (msg) {
            this.snackBar.open(`📚 ${msg}`, 'Fechar', { duration: 5000 });
          }
        });
      },
      error: err => {
        console.error('Erro ao obter o ID do cliente:', err);
      }
    });
  }

  title = 'EmprestaAi';
}
