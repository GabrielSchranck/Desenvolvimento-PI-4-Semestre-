import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Cartao } from '../models/Cliente';

@Injectable({
  providedIn: 'root'
})
export class CarteiraService {

  private readonly url: string = `${environment['apiUrl']}/Cartao`;

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

  private getHttpOptionsWithBody(body: any): any {
    const token = localStorage.getItem('authToken');

    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return {
      headers,
      body
    };
  }

  public async GetCartoes(): Promise<Observable<any>> {
    const apiUrl = `${this.url}/get`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions);
  }

  public async CreateCartao(cartao: Cartao): Promise<Observable<void>>{
    const apiUrl = `${this.url}/create`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<void>(apiUrl, cartao, httpOptions);
  }
}
