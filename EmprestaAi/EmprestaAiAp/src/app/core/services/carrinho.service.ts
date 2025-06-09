import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LivroAnunciadoDTO } from '../models/Livros';

@Injectable({
  providedIn: 'root'
})
export class CarrinhoService {

  private readonly url = `${environment['apiUrl']}/Carrinho`;

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

  public Create(): Promise<void> {
    const apiUrl = `${this.url}/create`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<void>(apiUrl, {}, httpOptions).toPromise()
      .catch(error => {
        console.error('Erro ao criar carrinho:', error);
        throw error;
      });
  }

  public VerificarCarrinhoExistente(): any {
    const apiUrl = `${this.url}/verify`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions).toPromise()
      .then(result => result !== undefined ? result : false)
      .catch(error => {
        console.error('Erro ao verificar carrinho existente:', error);
        throw error;
      });
  }

  public GetCarrinho(): Observable<any> {
    const apiUrl = `${this.url}/getCarrinho`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions);
  }

  public AddLivroToCarrinho(livroAnunciado: LivroAnunciadoDTO): Observable<any> {
    const apiUrl = `${this.url}/addItemCarrinho`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<{ retorno:string }>(apiUrl, livroAnunciado, httpOptions);
  }

  public RemoveItemCarrinho(id: number): Observable<any> {
    const apiUrl = `${this.url}/removeCarrinho/${id}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.delete(apiUrl, httpOptions);
  }
}
