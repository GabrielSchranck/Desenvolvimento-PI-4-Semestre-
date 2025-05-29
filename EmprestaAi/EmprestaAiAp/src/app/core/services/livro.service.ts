import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, firstValueFrom, map, Observable, throwError } from 'rxjs';
import { LivroDTO } from '../models/Livros';
import { CategoriasDTO } from '../models/Categorias';

@Injectable({
  providedIn: 'root'
})
export class LivroService {
  
  private readonly url = `${environment['apiUrl']}/Livro`;

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

  public async GetInfoLivro(titulo: string): Promise<Observable<any>>{
    const apiUrl = `${this.url}/getIinfoApi?name=${titulo}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions)
  }

  public async CreateLivro(formData: FormData): Promise<Observable<LivroDTO>> {
    const apiUrl = `${this.url}/create`;
    const token = localStorage.getItem('authToken');

    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.httpCliente.post<{ livroDTO: LivroDTO }>(apiUrl, formData, { headers }).pipe(
      map(response => response.livroDTO),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public GetCategoriasLivro(): Observable<any> {
    const apiUrl = `${this.url}/getCategorias`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public async update(formData: FormData): Promise<Observable<string>> {
    const apiUrl = `${this.url}/update`;
    const token = localStorage.getItem('authToken');

    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.httpCliente.put<{ retorno: string }>(apiUrl, formData, { headers }).pipe(
      map(response => response.retorno),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public async getLivros(): Promise<LivroDTO[]> {
    const apiUrl = `${this.url}/getLivros`;
    const httpOptions = this.getHttpOptions();

    try {
      const response = await firstValueFrom(
        this.httpCliente.get<{ livros: LivroDTO[] }>(apiUrl, httpOptions)
      );
      return response.livros;
    } catch (error: any) {
      if (error instanceof HttpErrorResponse && error.status === 400) {
        throw error.error;
      }
      throw new Error('Erro ao conectar com a API. ' + error.message);
    }
  }

  public delete(livroId: number): Observable<string> {
    const apiUrl = `${this.url}/delete/${livroId}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.delete<string>(apiUrl, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        console.log("Erro ao excluir livro: ", error.error);
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

}
