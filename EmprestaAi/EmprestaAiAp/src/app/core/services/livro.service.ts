import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, firstValueFrom, map, Observable, tap, throwError } from 'rxjs';
import { ComentarioLivroDTO, LivroAnunciadoDTO, LivroDTO } from '../models/Livros';
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

  public anunciaLivro(LivroAnunciadoDTO: any): Observable<any> {
    const apiUrl = `${this.url}/anunciar`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, LivroAnunciadoDTO, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public cancelarAnincio(anuncio: LivroAnunciadoDTO): Observable<any> {
    const apiUrl = `${this.url}/cancelarAnuncio`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, anuncio, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public GetAllLivrosAnunciados(): Observable<any> { 
    const apiUrl = `${this.url}/selecionarAnuncios`;
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

  public GetAllLivrosAnunciadosByCat(categoriaId: number, livroId: number, tipo: number): Observable<any> { 
    const apiUrl = `${this.url}/getRelacionados/${categoriaId}/${livroId}/${tipo}`;
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

  public GetLivrosAnunciados(anuncioId: number, tipo: number): Observable<any> {
    const apiUrl = `${this.url}/getLivroInfo/${anuncioId}/${tipo}`;
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

  public GetLivrosEmprestados(): Observable<any> {
    const apiUrl = `${this.url}/getLivrosEmprestados`;
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

  public DevolverLivro(livroId: number): Observable<any> {
    const apiUrl = `${this.url}/devolver/${livroId}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, {}, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public GetComentariosLivro(livroId: number): Observable<any> {
    const apiUrl = `${this.url}/obterComentarios/${livroId}`;
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

  public AdicionarComentarioLivro(comentario: ComentarioLivroDTO): Observable<any> {
    const apiUrl = `${this.url}/addicionarComentario`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, comentario, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public EditarComentarioLivro(comentario: ComentarioLivroDTO): Observable<any> {
    const apiUrl = `${this.url}/editarComentario/${comentario.id}/${comentario.comentario}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, comentario, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public DeletarComentarioLivro(comentarioId: number): Observable<any> {
    const apiUrl = `${this.url}/excluirComentario/${comentarioId}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post(apiUrl, {}, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }
}
