import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserAcessComponent } from '../../MainPages/user-acess/user-acess.component';
import { InputsComponent } from '../../Shered/inputs/inputs.component';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Cliente } from '../../core/models/Cliente';

// Declarar a variável global do Google Identity Services
declare const google: any;

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    UserAcessComponent,
    ReactiveFormsModule,
    InputsComponent,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChild('googleButtonContainer', { static: true })
  googleBtnContainer!: ElementRef<HTMLDivElement>;

  formularioLogin!: any;
  apiError: string = '';
  isLoading: boolean = false;

  public validacoes = {
    required: (campo: string) => `${campo} é obrigatório`,
    invalid:  (campo: string) => `${campo} inválido`,
    senha: 'A senha precisa ter no mínimo 6 dígitos'
  };

  constructor(
    private clienteService: ClienteService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.formularioLogin = new FormGroup({
      email:  new FormControl('', [Validators.required, Validators.email]),
      senha:  new FormControl('', [Validators.required, Validators.minLength(6)]),
      lembrar: new FormControl(false)
    });
  }

  ngAfterViewInit(): void {
    google.accounts.id.initialize({
      client_id: 'CHAVE AQUI',
      callback: (response: any) => this.handleCredentialResponse(response)
    });

    google.accounts.id.renderButton(
      this.googleBtnContainer.nativeElement,
      {
        theme: 'outline',
        size: 'large',
        width: 240,
        shape: 'rectangular',
        text: 'signin_with'
      }
    );

  }

  private handleCredentialResponse(response: any): void {
    const idToken = response.credential;
    this.isLoading = true;
    this.clienteService.loginWithGoogle(idToken)
      .subscribe({
        next: resp => {
          localStorage.setItem('authToken', resp.token);
          this.router.navigate(['']);
        },
        error: err => {
          this.apiError = err;
        }
      })
      .add(() => this.isLoading = false);
  }

  public Logar(): void {
    if (this.formularioLogin.invalid) {
      this.formularioLogin.markAllAsTouched();
      return;
    }

    this.apiError = '';
    this.isLoading = true;
    const cliente: Cliente = this.formularioLogin.value;

    this.clienteService.GetByEmailPassword(cliente).subscribe({
      next: retorno => {
        if (retorno.token) {
          localStorage.setItem('authToken', retorno.token);
          this.router.navigate(['']);
        } else {
          console.error('Erro: Token não recebido');
        }
        this.isLoading = false;
      },
      error: err => {
        this.apiError = err;
        this.isLoading = false;
      }
    });
  }

  public applyApiErrorsToForm(): void {
    if (this.apiError) {
      this.formularioLogin.get('email')?.setErrors({ apiError: this.apiError });
    }
  }
}
