import { Component, OnInit } from '@angular/core';
import { UserAcessComponent } from "../../MainPages/user-acess/user-acess.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputsComponent } from '../../Shered/inputs/inputs.component';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Router, RouterModule } from '@angular/router';
import { Cliente } from '../../core/models/Cliente';

@Component({
  selector: 'app-login',
  imports: [UserAcessComponent, ReactiveFormsModule, InputsComponent, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{

  formularioLogin: any;
  apiError: string = "";
  isLoading: boolean = false;

  public validacoes = {
    required: (campo:string) => `${campo} é obrigatório`,
    invalid: (campo:string) => `${campo} inválido`,
    senha: "A senha precisa ter no mínimo 6 dígitos",
  };

  ngOnInit(): void {
    this.formularioLogin = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      senha: new FormControl('', [Validators.required, Validators.minLength(6)]),
      lembrar: new FormControl('')
    });
  }

  constructor(private clienteService: ClienteService, private router: Router){}

  public async logarGoogle(): Promise<void>{

  }

  public async Logar(): Promise<void> {
    if(this.formularioLogin.invalid){
      this.formularioLogin.markAllAsTouched();
      return;
    }

    this.apiError = '';

    this.isLoading = true;
    const cliente: Cliente = this.formularioLogin.value;

    this.clienteService.GetByEmailPassword(cliente).subscribe({
      next: (retorno) => {
        if(retorno.token){
          localStorage.setItem("authToken", retorno.token);
          this.isLoading = false;
          this.apiError = '';
          this.router.navigate(['']);
        }
        else{
          console.error("Erro: Token não recebido");
        }
      },
      error: (err) => {
        this.apiError = err;
        this.isLoading = false;
      }
    });
  }

  public applyApiErrorsToForm(): void {
    console.log(this.apiError);
    if(this.apiError){
      this.formularioLogin.get('email').setErrors({apiError: this.apiError});
    }
  }
}
