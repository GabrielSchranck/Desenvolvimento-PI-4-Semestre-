import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { cpf } from 'cpf-cnpj-validator';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { firstValueFrom } from 'rxjs';
import { get } from 'http';
import { TextInputComponent } from "../../components/inputs/text-input/text-input.component";

@Component({
  selector: 'app-perfil',
  imports: [ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {

  formularioPerfil: FormGroup = new FormGroup({});
  cliente: Cliente = new Cliente();
  clienteLocal: any;

  constructor(private clienteService: ClienteService) {}

  async ngOnInit(): Promise<void> {
    
    this.clienteLocal = JSON.parse(localStorage.getItem("clienteDatas") || '{}');

    this.getPerfil();

    if(Object.keys(this.clienteLocal).length > 0){
      await this.setPerfil();
    }
    else{
      this.getPerfil();
    }

    this.formularioPerfil = new FormGroup({
      nome: new FormControl(this.cliente.Nome || '', [Validators.required]),
      email: new FormControl(this.cliente.Email || '', [Validators.required, Validators.email]),
      cpf: new FormControl(this.cliente.Cpf || ''),
      idade: new FormControl(this.cliente.Idade || 0),
      dataNascimento: new FormControl(this.cliente.DataNascimento || ''), 
      ddd: new FormControl(this.cliente.DDD || ''),
      contato: new FormControl(this.cliente.Contato || ''),
    });
  }


  private async getPerfil(): Promise<void> {
    this.clienteService.GetByToken().subscribe({
      next: (retorno) => {
        if(retorno){
          localStorage.setItem("clienteDatas", JSON.stringify(retorno));
        }
        else{
          console.log("Erro ao obter cliente");
        }
      },
      error: (erro) => {
        console.log("Erro ao conectar a API: " + erro);
      }
    })
  }

  private async setPerfil(): Promise<void> {
    if (this.clienteLocal) {
      this.cliente.Nome = this.clienteLocal.cliente.nome;
      this.cliente.Email = this.clienteLocal.cliente.email;
      this.cliente.Cpf = this.clienteLocal.cliente.cpf;
      this.cliente.Idade = Number(this.clienteLocal.cliente.idade) || 0; // Garante que seja número
      this.cliente.DDD = this.clienteLocal.cliente.ddd;
      this.cliente.Contato = this.clienteLocal.cliente.contato;
      
      // Converter data para YYYY-MM-DD se for válida
      if (this.clienteLocal.cliente.dataNascimento) {
        this.cliente.DataNascimento = this.clienteLocal.cliente.dataNascimento.split('T')[0];
      } else {
        this.cliente.DataNascimento = '';
      }
    }
}

  
}

