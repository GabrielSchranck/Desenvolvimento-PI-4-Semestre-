import { Injectable } from '@angular/core';
import { Endereco } from '../models/Cliente';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EnderecoService {

  private readonly url = `${environment["apiUrl"]}/cliente`;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  };

  constructor(private http: HttpClient) { }

  public createEndereco(endereco: Endereco): Observable<{ endereco: Endereco }> {
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

    const urlApi = `${this.url}/endereco`;
    return this.http.post<{ endereco: Endereco }>(urlApi, endereco, this.httpOptions);
  }
}
