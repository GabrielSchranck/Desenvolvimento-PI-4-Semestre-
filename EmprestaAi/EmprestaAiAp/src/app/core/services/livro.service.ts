import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

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
    
    console.log(titulo)

    return this.httpCliente.get<any>(apiUrl, httpOptions)
  }
}
