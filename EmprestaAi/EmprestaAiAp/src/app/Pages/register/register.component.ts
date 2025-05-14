import { Component, OnInit } from '@angular/core';
import { UserAcessComponent } from "../../MainPages/user-acess/user-acess.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Router, RouterModule } from '@angular/router';
import { Cliente } from '../../core/models/Cliente';
import { cpf } from 'cpf-cnpj-validator';
import { InputsComponent } from '../../Shered/inputs/inputs.component';

@Component({
  selector: 'app-register',
  imports: [UserAcessComponent, ReactiveFormsModule, RouterModule, InputsComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {

  constructor(private clienteService : ClienteService, private router: Router){}

  formularioRegistro: any;
  clienteRetornado: any;
  apiErrors: {[key:string] : string} = {}
  isClicked: boolean = false;

  public validacoes = {
    required: (campo:string) => `${campo} é obrigatório`,
    invalid: (campo:string) => `${campo} inválido`,
    senha: "A senha precisa ter no mínimo 6 dígitos",
    ddd: "DDD não encontrado"
  };

  ngOnInit(): void {
    this.formularioRegistro = new FormGroup({
      nome: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      cpf: new FormControl('', [Validators.required, this.cpfInvalid]),
      dataNascimento: new FormControl('', [Validators.required]),
      contato: new FormControl('', [Validators.required]),
      senha: new FormControl('', [Validators.required])
    });
  }

  public cpfInvalid(control: any){
    const isValid = cpf.isValid(control.value);
    return isValid ? null : { invalidCpf: true }
  }

  public CadastrarCliente(): void{

    this.isClicked = true;

    if(this.formularioRegistro.invalid){
      this.formularioRegistro.markAllAsTouched();
      this.isClicked = false;
      return;
    }

    const cliente: Cliente = this.formularioRegistro.value;

    this.clienteService.CreateClient(cliente).subscribe({
      next: (retorno) => {
        if(retorno.token){
          localStorage.setItem('authToken', retorno.token);
          this.apiErrors = {};
          this.isClicked = false;
          this.router.navigate(['']);
        }
        else{
          this.isClicked = false;
          console.error("Erro: Token não recebido");
        }
      },
      error: (err) => {
        console.log(err);
        console.log("Erro erro super erro")
        if(err.errors){
          this.isClicked = false;
          this.apiErrors = err.errors;
          this.applyApiErrorsToForm();
        }
      }
    });
  }

  public applyApiErrorsToForm(): void{
    for(const field in this.apiErrors){
      const control = this.formularioRegistro.get(field);
      if(control){
        control.setErrors({ apiError: this.apiErrors[field]});
      }
    }
  }

}
