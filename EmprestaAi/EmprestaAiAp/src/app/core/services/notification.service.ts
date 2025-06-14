import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, catchError, throwError } from 'rxjs';
import { environment } from '../../../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private readonly url = `${environment['apiUrl']}/Cliente`;

  private hubConnection!: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<string | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  constructor(private httpCliente: HttpClient) { }

  public startConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://seuservidor.com/notificationhub?userId=${userId}`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error('Erro SignalR:', err));

    this.hubConnection.on('ReceiveNotification', (mensagem: string) => {
      this.notificationSubject.next(mensagem);
    });
  }

  private getHttpOptions(): { headers: HttpHeaders } {
    const token = localStorage.getItem('authToken');

    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return { headers };
  }

  public closeNotificacao(notificacaoId: number): any {
    const apiUrl = `${this.url}/fecharNotificacao/${notificacaoId}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<any>(apiUrl, {}, httpOptions).pipe(
      catchError((error) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }
}
