import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ClienteDTO } from '../models/Cliente';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  cliente: any;

  constructor(private authService: AuthService) { }

  getCliente(){
    this.cliente = this.authService.cliente$;
  }
  
  CreateUser(cliente:ClienteDTO){
    
  }

}
