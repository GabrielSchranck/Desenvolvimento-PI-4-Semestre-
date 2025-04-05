import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = `${environment['apiUrl']}/Auth`;

  constructor(private router: Router, private httpClient: HttpClient) {
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

  private getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  public isLoggedIn(): Observable<boolean> {
    const urlApi = `${this.apiUrl}/verify`;
  
    const token = this.getToken();
    const options = this.getHttpOptions();
  
    if (!token) return of(false); 
  
    return this.httpClient.get<{ retorno: boolean }>(urlApi, options).pipe(
      map(response => response.retorno === true),
      catchError(() => of(false)) 
    );
  }
  

  public login(token: string): void{
    localStorage.setItem('authToken', token);
  }

  public logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('clienteDatas');
  }

  public isTokenValid(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const tokenPayload = this.decodeToken(token);
    if (tokenPayload && tokenPayload.exp) {
      return tokenPayload.exp * 1000 > Date.now();
    }

    return false;
  }

  private decodeToken(token: string): any {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  }
}
