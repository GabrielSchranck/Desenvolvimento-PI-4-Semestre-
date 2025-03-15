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
    if (typeof window !== 'undefined' && window.localStorage) {
      this.clienteLocal = JSON.parse(localStorage.getItem("clienteDatas") || '{}');
    }
    
    this.formularioPerfil = new FormGroup({
      nome: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      cpf: new FormControl(''),
      idade: new FormControl(''),
      dataNascimento: new FormControl(''),
      ddd: new FormControl( ''),
      contato: new FormControl(''),
    });

    console.log(this.clienteLocal)

    if(this.clienteLocal){
      await this.setPerfil();
    }
    else{
      this.getPerfil();
    }
  }


  private async getPerfil(): Promise<void> {
    this.clienteService.GetByToken().subscribe({
      next: (retorno) => {
        if(retorno){
          if (typeof window !== 'undefined' && window.localStorage) {
            localStorage.setItem("clienteDatas", JSON.stringify(retorno));
          }
        }
        else{
          console.log("Erro ao obter cliente");
        }
      },
      error: (erro) => {
        console.log("Erro ao conectar à API: " + erro);
      }
    })
  }

  private async setPerfil(): Promise<void>{
    // "{"cliente":{"id":1,"nome":"Gabriel Schranck","email":"gabriel.futurisss@gmail.com","cpf":"44807757814","idade":0,"ddd":19,"contato":"912345678","dataNascimento":"2003-06-02T00:00:00"},"enderecos":[]}"
    if(this.clienteLocal){
      this.cliente.Nome = this.clienteLocal.nome;
      this.cliente.Email = this.clienteLocal.email;
      this.cliente.Cpf = this.clienteLocal.cpf;
      this.cliente.Idade = this.clienteLocal.idade;
      this.cliente.DDD = this.clienteLocal.ddd;
      this.cliente.Contato = this.clienteLocal.contato;
      this.cliente.DataNascimento = this.clienteLocal.dataNascimento;
    }

    console.log(this.cliente)
  }
  
}

