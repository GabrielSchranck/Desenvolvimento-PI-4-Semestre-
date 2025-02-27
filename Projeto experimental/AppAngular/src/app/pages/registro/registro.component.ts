import { Component, OnInit } from '@angular/core';
import { UseracessComponent } from '../../components/useracess/useracess.component';
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms'
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { Router } from '@angular/router';

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
      nome: new FormControl(''),
      email: new FormControl(''),
      cpf: new FormControl(''),
      dataNascimento: new FormControl(''),
      genero: new FormControl(''),  
      ddd: new FormControl(''),
      contato: new FormControl(''),
      senha: new FormControl('')
    });
  }

  opcoesGenero: string[] = ["Masculino", "Feminino", "Outro"];

  CadastrarCliente(): void{
    const cliente :Cliente = this.formularioRegistro.value;
    this.clienteService.CreateClient(cliente).subscribe((retorno) => {
      this.clienteRetornado = retorno;
      localStorage.setItem('authToken', this.clienteRetornado.token);
      this.router.navigate(['']);
    });
  }

}
