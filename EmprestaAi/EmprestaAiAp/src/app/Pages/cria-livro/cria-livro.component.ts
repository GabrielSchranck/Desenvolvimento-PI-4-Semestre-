import { Component, OnInit } from '@angular/core';
import { LivroDTO } from '../../core/models/Livros';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { LivroService } from '../../core/services/livro.service';
import Swal from 'sweetalert2';
import { firstValueFrom } from 'rxjs';
import { InputsComponent } from "../../Shered/inputs/inputs.component";
import { CategoriasDTO } from '../../core/models/Categorias';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cria-livro',
  imports: [ReactiveFormsModule, NavbarComponent, InputsComponent],
  templateUrl: './cria-livro.component.html',
  styleUrl: './cria-livro.component.css'
})
export class CriaLivroComponent implements OnInit {

  livro: LivroDTO = new LivroDTO();
  categorias: CategoriasDTO[] = [];
  formularioLivro!: FormGroup;
  selectedFile: File | null = null;

  constructor(private formBuilder: FormBuilder, private livroService: LivroService, private router: Router) { }

  public ngOnInit(): void {
    this.CreateFormularioLivro();
    this.GetCategoriasLivro();
  }

  public CreateFormularioLivro(): void {
    this.formularioLivro = this.formBuilder.group({
      id: [null],                    
      clienteId: [null],
      titulo: [''],
      valor: [null],
      custo: [null],
      qtdPaginas: [null],
      quantidade: [null],
      uriImagemLivro: [''],
      categoriaId: [null] 
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.formularioLivro.get('uriImagemLivro')?.setValue(e.target.result); 
    };
    reader.readAsDataURL(file);
  }

  public async BuscaLivrosInfo(): Promise<void> {
    const nomeLivro: string = this.formularioLivro?.get("titulo")?.value;
    const imgLivro: string = this.formularioLivro?.get("uriImagemLivro")?.value;

    if (nomeLivro === "") return;

    if (imgLivro !== "") return;

    (await this.livroService.GetInfoLivro(nomeLivro)).subscribe(
      (response) => {
        const imgPathResult = response;

        this.formularioLivro.patchValue({
          uriImagemLivro: imgPathResult.urlImagem
        });
      }
    );
  }

  public async CriarLivro(): Promise<void> {
    if (!this.formularioLivro) return;

    const livroCriadoForm = this.formularioLivro.value;
    const formData = new FormData();

    formData.append('Id', livroCriadoForm.id?.toString() ?? '');
    formData.append('ClienteId', livroCriadoForm.clienteId?.toString() ?? '');
    formData.append('CategoriaId', livroCriadoForm.categoriaId?.toString() ?? '');
    formData.append('Titulo', livroCriadoForm.titulo ?? '');
    formData.append('Valor', livroCriadoForm.valor?.toString() ?? '');
    formData.append('Custo', livroCriadoForm.custo?.toString() ?? '');
    formData.append('QtdPaginas', livroCriadoForm.qtdPaginas?.toString() ?? '');
    formData.append('Quantidade', livroCriadoForm.quantidade?.toString() ?? '');
    formData.append('UriImagemLivro', livroCriadoForm.uriImagemLivro ?? '');

    if (this.selectedFile) {
      formData.append('Imagem', this.selectedFile);
    }

    (await this.livroService.CreateLivro(formData)).subscribe(
      (response) => {
        Swal.fire({
          title: 'Erro',
          text: (typeof response === 'string' ? response : 'Erro ao criar livro.'),
          icon: 'error',
          confirmButtonText: 'OK'
        });
      },
      (error) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Livro criado com sucesso!',
          icon: 'success',
          confirmButtonText: 'OK'
        }).then(() => {
          this.router.navigate(['/livros']);
        });
      }
    );
  }



  public DeletaImagem(): void {
    this.formularioLivro.patchValue({
      uriImagemLivro: ""
    });
  }

  public GetCategoriasLivro(): void {
    this.livroService.GetCategoriasLivro().subscribe(
      (response) => {
        this.categorias = response.result;
      },
      (error) => {
        console.error("Erro ao buscar categorias:", error);
      }
    );
  }

  public GoToLivros(): void {
    this.router.navigate(['/livros']);
  }
}

