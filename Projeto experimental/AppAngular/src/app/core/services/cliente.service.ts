import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  cliente: any;

  constructor(private authService: AuthService) { }

  getCliente(){
    this.cliente = this.authService.cliente$;
  }
  
}
