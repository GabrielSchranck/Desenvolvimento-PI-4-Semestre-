import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Cliente } from '../models/Cliente';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  url = 'https://localhost:7010/api/cliente'

  constructor(private http: HttpClient) { }

  GetAll() : Observable<Cliente[]>{
    return this.http.get<Cliente[]>(this.url);
  }

  GetById(id: number) :Observable<Cliente>{
    const urlApi = `${this.url}/${id}`;
    return this.http.get<Cliente>(urlApi);
  }

  GetByEmailPassword(email: string, senha: string) : Observable<any>{
    const urlApi = `${this.url}/login`
    return this.http.post(urlApi, {email, senha} ,httpOptions);
  }

  CreateClient(cliente: Cliente) : Observable<any>{
    const urlApi = `${this.url}/create`
    return this.http.post<Cliente>(urlApi, cliente, httpOptions);
  }

  UpdateClient(cliente: Cliente) : Observable<any>{
    return this.http.put<Cliente>(this.url, cliente, httpOptions);
  }

  Delete(id: number) : Observable<any>{
    const apiUrl = `${this.url}/${id}`;
    return this.http.delete<number>(apiUrl, httpOptions);
  }
}
