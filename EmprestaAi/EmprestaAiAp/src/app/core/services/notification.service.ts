import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private hubConnection!: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<string | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  startConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://seuservidor.com/notificationhub?userId=${userId}`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error('Erro SignalR:', err));

    this.hubConnection.on('ReceiveNotification', (mensagem: string) => {
      this.notificationSubject.next(mensagem);
    });
  }
}
