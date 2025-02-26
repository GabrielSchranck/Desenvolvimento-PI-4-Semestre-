import { Component } from '@angular/core';
import { UseracessComponent } from '../../components/useracess/useracess.component';
import { RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms'

@Component({
  selector: 'app-registro',
  imports: [UseracessComponent, RouterModule, ReactiveFormsModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {

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

  opcoesGenero = [
    {valor: "masculino", texto: "Masculino"},
    {valor: "feminino", texto: "Feminino"},
    {valor: "outro", texto: "Outro"}
  ];

}
