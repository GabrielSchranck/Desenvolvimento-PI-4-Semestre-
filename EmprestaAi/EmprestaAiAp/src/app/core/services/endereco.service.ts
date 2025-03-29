import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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

  public async CreateEnderecoCliente(): Promise<void>{
    
  }
}
