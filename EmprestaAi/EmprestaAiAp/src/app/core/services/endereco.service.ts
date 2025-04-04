import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { Endereco } from '../models/Endereco';
import { environment } from '../../../../environments/environments';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class EnderecoService {

  private readonly url = `${environment['apiUrl']}/Endereco`;

  constructor(private httpCliente: HttpClient) { }

  public async GetEnderecoByViaCep(cep:string): Promise<Observable<Endereco>>{
    const apiUrl = `${this.url}/getViaCep?cep=${cep}`
    return this.httpCliente.get<Endereco>(apiUrl, httpOptions)
  }

  public CreateEnderecoCliente(endereco: Endereco): Observable<void> {
    const apiUrl = `${this.url}/create`;

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

    const retorno = this.httpCliente.post<void>(apiUrl, endereco, httpOptions);

    console.log(retorno)

    return retorno;
  }

  public DeleteEnderecoCliente(endereco: Endereco): Observable<void> {
    const apiUrl = `${this.url}/delete`;

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

    return this.httpCliente.post<void>(apiUrl, endereco, httpOptions);
  }
}
