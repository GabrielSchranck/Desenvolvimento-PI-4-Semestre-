import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LivroAnunciadoDTO } from '../models/Livros';
import { Operacao } from '../models/Operacao';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OperacoesService {

  private readonly url = `${environment['apiUrl']}/Pagamento`;

  constructor(private httpCliente: HttpClient) { }

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

  ComprarLivro(operacao: Operacao) {
    const apiUrl = `${this.url}/comprarLivro`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<any>(apiUrl, operacao, httpOptions).pipe(
      catchError((error) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }
}
