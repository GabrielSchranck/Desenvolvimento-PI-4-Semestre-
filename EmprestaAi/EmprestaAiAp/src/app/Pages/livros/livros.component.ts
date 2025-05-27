import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivroDTO } from '../../core/models/Livros';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LivroService } from '../../core/services/livro.service';

@Component({
  selector: 'app-livros',
  imports: [UserInfoComponent],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent implements OnInit {
  livros: LivroDTO[] = [];
  livrosEmprestados: LivroDTO[] = [];
  abrirModal: boolean = false;

  constructor(private router: Router, private formBuilder: FormBuilder, private livroService: LivroService){}
  
  async ngOnInit(): Promise<void> {
    this.buscarLivros();
  }

  public GoToAddLivro(){
    this.router.navigate(['/registraLivro']);
  }

  public async buscarLivros() {
    try {
      const data: LivroDTO[] = await this.livroService.getLivros();
      this.livros = data;
    } catch (error) {
      console.error('Erro ao buscar livros:', error);
    }
  }


  
}
