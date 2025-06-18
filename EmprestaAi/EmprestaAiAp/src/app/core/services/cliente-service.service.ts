import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../../environments/environments';
import { Cliente } from '../models/Cliente';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  private readonly url = `${environment["apiUrl"]}/cliente`;
  httpCliente: any;

  constructor(private http: HttpClient) { }

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

  GetAll() : Observable<Cliente[]>{
    return this.http.get<Cliente[]>(this.url);
  }

  GetByToken(): Observable<any> {

    const token = localStorage.getItem('authToken');

    if (!token) {
      return throwError(() => new Error('Token de autenticação não encontrado.'));
    }

    const urlApi = `${this.url}/perfil`;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      })
    };

    return this.http.get<Cliente>(urlApi, httpOptions)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            return throwError(() => error.error);
          }
          return throwError(() => "Erro desconhecido.\n" + error.message);
        })
      );
  }

  GetByEmailPassword(cliente: Cliente) : Observable<any>{
    const urlApi = `${this.url}/login`
    return this.http.post<{cliente: Cliente, token: string}>(urlApi, cliente ,httpOptions)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status === 400){
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  CreateClient(cliente: Cliente) : Observable<{cliente: Cliente, token : string}>{
    const urlApi = `${this.url}/create`
    return this.http.post<{cliente: Cliente, token: string}>(urlApi, cliente, httpOptions)
    .pipe(
      catchError((error) => {
        if(error.status === 400){
          return throwError(() => error.error);
        }
        return throwError(() => "Erro desconhecido.\n" + error.message);
      })
    );
  }

  UpdateClient(cliente: Cliente) : Observable<any>{
    const urlApi = `${this.url}/update`;

    const token = localStorage.getItem('authToken');

    if (!token) {
      return throwError(() => new Error('Token de autenticação não encontrado.'));
    }

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      })
    };

    return this.http.put<Cliente>(urlApi, cliente, httpOptions);
  }

  Delete(id: number) : Observable<any>{
    const apiUrl = `${this.url}/${id}`;
    return this.http.delete<number>(apiUrl, httpOptions);
  }

  loginWithGoogle(idToken: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(
      `${this.url}/google-login`,
      { idToken }
    );
  }

  public sacarSaldo(valor: number): Observable<any> {
    const apiUrl = `${this.url}/sacar/${valor}`;
    const httpOptions = this.getHttpOptions();

    return this.http.post<any>(apiUrl, {}, httpOptions);
  }
}
