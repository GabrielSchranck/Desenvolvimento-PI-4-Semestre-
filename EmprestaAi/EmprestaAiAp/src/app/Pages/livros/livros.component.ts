import { Component } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivroDTO } from '../../core/models/Livros';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-livros',
  imports: [UserInfoComponent],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent {
  livros: LivroDTO[] = [];
  livrosEmprestados: LivroDTO[] = [];
  abrirModal: boolean = false;

  constructor(private router: Router, private formBuilder: FormBuilder){}

  

}
