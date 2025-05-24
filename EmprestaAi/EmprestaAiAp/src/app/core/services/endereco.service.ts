import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { Endereco } from '../models/Endereco';
import { environment } from '../../../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class EnderecoService {

  private readonly url = `${environment['apiUrl']}/Endereco`;

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

  public async GetEnderecoByViaCep(cep:string): Promise<Observable<Endereco>>{
    const apiUrl = `${this.url}/getViaCep?cep=${cep}`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.get<Endereco>(apiUrl, httpOptions)
  }

  public CreateEnderecoCliente(endereco: Endereco): Observable<void> {
    const apiUrl = `${this.url}/create`;
    const httpOptions = this.getHttpOptions();

    console.log("Enviando para API:", JSON.stringify(endereco));


    return this.httpCliente.post<void>(apiUrl, endereco, httpOptions).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status === 400){
          return throwError(() => error.error);
        }
        return throwError(() => "Erro ao conectar com a API.");
      })
    );
  }

  public DeleteEnderecoCliente(endereco: Endereco): Observable<void> {
    const apiUrl = `${this.url}/delete`;
    const httpOptions = this.getHttpOptionsWithBody(endereco);

    return this.httpCliente.request<void>('DELETE', apiUrl, httpOptions).pipe(
      map(() => void 0)
    );
  }

  public UpdateEnderecoCliente(endereco: Endereco): Observable<any>{
    const apiUrl = `${this.url}/update`;
    const httpOptions = this.getHttpOptions();

    return this.httpCliente.put<Endereco>(apiUrl, endereco, httpOptions);
  }
}
