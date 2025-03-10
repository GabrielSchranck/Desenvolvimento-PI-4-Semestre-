import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(private router: Router) {
  }

  getToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('authToken');
    }
    return null;
  }

  isLoggedIn(): boolean{
    const token = this.getToken();
    return !!token;
  }

  login(token: string): void{
    localStorage.setItem('authToken', token);
  }

  logout(): void {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('authToken');
    }
  }

  isTokenValid(): boolean {
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
