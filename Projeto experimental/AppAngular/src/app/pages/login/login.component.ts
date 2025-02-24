import { Component } from '@angular/core';
import { DefaultLoginLayoutComponent } from '../../components/default-login-layout/default-login-layout.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputPrimaryComponent } from '../../components/input-primary/input-primary.component';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  imports: [
    DefaultLoginLayoutComponent,
    ReactiveFormsModule,
    InputPrimaryComponent
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup; 

  constructor() {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  logar() {
    // lógica de login
  }
}
