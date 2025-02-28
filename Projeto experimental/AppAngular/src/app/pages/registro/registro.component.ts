import { Component, OnInit } from '@angular/core';
import { UseracessComponent } from '../../components/useracess/useracess.component';
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { Router } from '@angular/router';
import { cpf } from 'cpf-cnpj-validator';

@Component({
  selector: 'app-registro',
  imports: [UseracessComponent, RouterModule, ReactiveFormsModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent implements OnInit{

  constructor(private clienteService : ClienteService, private router: Router){}

  formularioRegistro: any;
  clienteRetornado: any;

  ngOnInit(): void {

    this.formularioRegistro = new FormGroup({
      nome: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      cpf: new FormControl('', [Validators.required, this.cpfInvalid]),
      dataNascimento: new FormControl('', [Validators.required]),
      genero: new FormControl('', [Validators.required]),  
      ddd: new FormControl('', [Validators.required]),
      contato: new FormControl('', [Validators.required]),
      senha: new FormControl('', [Validators.required])
    });
  }

  public validacoes = {
    required: (campo:string) => `${campo} é obrigatório`,
    invalid: (campo:string) => `${campo} inválido`,
    senha: "A senha precisa ter no mínimo 6 dígitos",
    ddd: "DDD não encontrado"
  };
  opcoesGenero: string[] = ["Masculino", "Feminino", "Outro"];

  cpfInvalid(control: any){
    const isValid = cpf.isValid(control.value);
    return isValid ? null : { invalidCpf: true }
  }

  CadastrarCliente(): void{
    const cliente :Cliente = this.formularioRegistro.value;
    this.clienteService.CreateClient(cliente).subscribe((retorno) => {
      this.clienteRetornado = retorno;
      localStorage.setItem('authToken', this.clienteRetornado.token);
      this.router.navigate(['']);
    });

    this.formularioRegistro.markAllAsTouched();
  }

}
