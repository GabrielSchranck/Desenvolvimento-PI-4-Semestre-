import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { cpf } from 'cpf-cnpj-validator';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';

@Component({
  selector: 'app-perfil',
  imports: [ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {

  formularioPerfil: any;

  constructor(private clienteService: ClienteService) { }

  ngOnInit(): void {
    this.formularioPerfil = new FormGroup({
      nome: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      cpf: new FormControl(''),
      idade: new FormControl(''),
      dataNascimento: new FormControl(''),
      genero: new FormControl(''),
      ddd: new FormControl(''),
      contato: new FormControl(''),
    });
  }

  private async getPerfil(id: number): Promise<Cliente> {
    const cliente = await this.clienteService.GetById(id).toPromise();
    if (!cliente) {
      throw new Error('Cliente not found');
    }
    return cliente;
  }


}
