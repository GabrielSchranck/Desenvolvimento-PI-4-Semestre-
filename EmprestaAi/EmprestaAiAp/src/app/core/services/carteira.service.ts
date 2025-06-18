import { map, Observable } from 'rxjs';
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

  public async DeleteCartao(cartao: Cartao): Promise<Observable<void>>{
    const apiUrl = `${this.url}/delete`;
    const httpOptions = this.getHttpOptionsWithBody(cartao);

    return this.httpCliente.request<void>('DELETE', apiUrl, httpOptions).pipe(
      map(() => void 0)
    );
  }

  public async getUUID(): Promise<{ uuid: string }> {
    const apiUrl = `${this.url}/getUuidMP`;
    const httpOptions = this.getHttpOptions();

    const result = await this.httpCliente.get<{ uuid: string }>(apiUrl, httpOptions).toPromise();
    if (!result) {
      throw new Error('UUID não encontrado.');
    }
    return result;
  }

  public createUUID(uuid: string): Promise<void> {
    const apiUrl = `${this.url}/creteUuid`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.post<void>(apiUrl, JSON.stringify(uuid), httpOptions).toPromise().catch(
      (error: any) => {
        console.error('Erro ao criar UUID:', error);
        throw error;
      }
    );
  }

  public addSaldo(valor: number): void {
    const apiUrl = `${environment['apiUrl']}/Pagamento/criar-pagamento/${valor}`;
    const httpOptions = this.getHttpOptions();

    this.httpCliente.post<any>(apiUrl, {
      valor: valor
    }, httpOptions).subscribe(res => {
      window.location.href = res.result;
    });
  }

  public getSaldo(): Observable<any> {
    const apiUrl = `${this.url}/getSaldo`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<any>(apiUrl, httpOptions);
  }
}
