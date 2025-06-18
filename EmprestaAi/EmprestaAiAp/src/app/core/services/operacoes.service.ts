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

  public ComprarLivro(operacoes: Operacao[]) {
    const apiUrl = `${this.url}/comprarLivro`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<any>(apiUrl, operacoes, httpOptions).pipe(
      catchError((error) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public SolicitarEmprestimoDoacao(Operacoes: Operacao[]) {
    const apiUrl = `${this.url}/solicitarEmprestimoDoacao`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<any>(apiUrl, Operacoes, httpOptions).pipe(
      catchError((error) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public FinalizarOperacaoEmprestimoDoacao(operacoes: Operacao[], clienteId: number) {
    const apiUrl = `${this.url}/finalizarOperacaoEmprestimoDocao?clienteId=${clienteId}`;
    const httpOptions = this.getHttpOptions(); 

    return this.httpCliente.post<any>(apiUrl, operacoes, httpOptions).pipe(
      catchError((error) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

}
