import { Component, OnInit } from '@angular/core';
import { UseracessComponent } from "../../components/useracess/useracess.component";
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [UseracessComponent, RouterModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  formularioLogin: any;

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

  Logar(): void {

  }

}
