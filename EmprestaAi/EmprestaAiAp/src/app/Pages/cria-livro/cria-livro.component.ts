import { Component, OnInit } from '@angular/core';
import { LivroDTO } from '../../core/models/Livros';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { LivroService } from '../../core/services/livro.service';

@Component({
  selector: 'app-cria-livro',
  imports: [ReactiveFormsModule, NavbarComponent],
  templateUrl: './cria-livro.component.html',
  styleUrl: './cria-livro.component.css'
})
export class CriaLivroComponent implements OnInit {

  livro: LivroDTO = new LivroDTO();
  formularioLivro!: FormGroup;

  constructor(private formBuilder: FormBuilder, private livroService: LivroService) {}

  public ngOnInit(): void {
    this.CreateFormularioLivro();
  }

  public CreateFormularioLivro(): void {
    this.formularioLivro = this.formBuilder.group({
      id: [''],
      clienteId: [''],
      autorId: [''],
      titulo: [''],
      valor: [''],
      custo: [''],
      qtdPaginas: [''],
      quantidade: [''],
      uriImagemLivro: ['']
    });
  }

  public async BuscaLivrosInfo(): Promise<void>{
    const nomeLivro: string = this.formularioLivro?.get("titulo")?.value;
    const imgLivro:string = this.formularioLivro?.get("uriImagemLivro")?.value;
    
    if(nomeLivro === "") return;

    if(imgLivro !== "") return;

    (await this.livroService.GetInfoLivro(nomeLivro)).subscribe(
      (response) => {
        const imgPathResult = response;

        this.formularioLivro.patchValue({
          uriImagemLivro: imgPathResult.urlImagem
        });
      }
    );
  }

  public async CriarLivro(): Promise<void>{

  }

  public DeletaImagem(): void{
    this.formularioLivro.patchValue({
      uriImagemLivro:""
    });
  }
}

