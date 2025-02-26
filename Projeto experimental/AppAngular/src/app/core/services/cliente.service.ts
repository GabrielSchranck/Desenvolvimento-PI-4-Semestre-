import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ClienteDTO } from '../models/Cliente';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  cliente: any;

  constructor(private authService: AuthService, private http: HttpClient) { }

  getCliente(){
    this.cliente = this.authService.cliente$;
  }

  CreateUser(cliente:ClienteDTO){
    const clienteApiHttp = environment.apiUrl + "/clientes";

    return this.http.post(clienteApiHttp, cliente);
  }

}
