import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    private clienteSubject = new BehaviorSubject<any>(null);
    cliente$ = this.clienteSubject.asObservable();

  constructor() { }

  setCliente(cliente: any){
    this.clienteSubject.next(cliente);
  }

  getCliente(){
    return this.clienteSubject.getValue();
  }
}
