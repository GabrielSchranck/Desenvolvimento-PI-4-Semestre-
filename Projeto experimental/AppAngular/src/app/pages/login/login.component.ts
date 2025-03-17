import { Component, OnInit } from '@angular/core';
import { UseracessComponent } from "../../components/useracess/useracess.component";
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { TextInputComponent } from "../../components/inputs/text-input/text-input.component";

@Component({
  selector: 'app-login',
  imports: [UseracessComponent, RouterModule, ReactiveFormsModule, TextInputComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  formularioLogin: any;
  apiError: string = "abacaxi";
  isLoading: boolean = false;

  constructor(private clienteService: ClienteService, private router: Router){}

  ngOnInit(): void {
    this.formularioLogin = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      senha: new FormControl('', [Validators.required, Validators.minLength(6)]),
      lembrar: new FormControl('')
    });
  }

  public validacoes = {
    required: (campo:string) => `${campo} é obrigatório`,
    invalid: (campo:string) => `${campo} inválido`,
    senha: "A senha precisa ter no mínimo 6 dígitos",
  };

  async logarGoogle(): Promise<void>{
    //Programar logar com o google
  }

  async Logar(): Promise<void> {
    if(this.formularioLogin.invalid){
      this.formularioLogin.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const cliente: Cliente = this.formularioLogin.value;

    this.clienteService.GetByEmailPassword(cliente).subscribe({
      next: (retorno) => {
        if(retorno.token){
          localStorage.setItem("authToken", retorno.token);
          this.isLoading = false;
          this.apiError = '';
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
              console.error(erro);
            }
          });
          this.router.navigate(['']);
        }
        else{
          console.error("Erro: Token não recebido");
        }
      },
      error: (err) => {
        this.apiError = err;
        this.isLoading = false;
        this.applyApiErrorsToForm();
      }
    });


  }

  applyApiErrorsToForm(): void {
    console.log(this.apiError);
    if(this.apiError){
      this.formularioLogin.get('email').setErrors({apiError: this.apiError});
    }
  }
}
