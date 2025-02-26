import { Component } from '@angular/core';
import { UseracessComponent } from '../../components/useracess/useracess.component';
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms'
import { ClienteDTO } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';

@Component({
  selector: 'app-registro',
  imports: [UseracessComponent, RouterModule, ReactiveFormsModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {

  constructor(private clienteService : ClienteService){}

  formularioRegistro = new FormGroup({
    nome: new FormControl(''),
    email: new FormControl(''),
    cpf: new FormControl(''),
    dataNascimento: new FormControl(''),
    genero: new FormControl(''),  
    ddd: new FormControl(''),
    contato: new FormControl(''),
    senha: new FormControl('')
  });

  opcoesGenero: string[] = ["Masculino", "Feminino", "Outro"];

  Cadastrar(){
    const cliente : ClienteDTO = {
      Nome: this.formularioRegistro.get('nome')?.value ?? undefined,
      Email: this.formularioRegistro.get('email')?.value ?? undefined,
      Cpf: this.formularioRegistro.get('cpf')?.value ?? undefined,
      //DataNascimento: this.formularioRegistro.get('dataNascimento')?.value ? new Date(this.formularioRegistro.get('dataNascimento')?.value) : undefined,
      Genero: this.formularioRegistro.get('genero')?.value === "Masculino" ? 1 : this.formularioRegistro.get('genero')?.value === "Masculino" ? 2 : 3,
      DDD: this.formularioRegistro.get('ddd')?.value ? Number(this.formularioRegistro.get('ddd')?.value) : undefined,
      Contato: this.formularioRegistro.get('contato')?.value ?? undefined,
      Senha: this.formularioRegistro.get('senha')?.value ?? undefined
    }

    this.clienteService.CreateUser(cliente)
  }

}
