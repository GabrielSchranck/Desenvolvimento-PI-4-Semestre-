import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivroDTO } from '../../core/models/Livros';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { LivroService } from '../../core/services/livro.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-livros',
  imports: [UserInfoComponent, ReactiveFormsModule],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent implements OnInit {

  livro!: LivroDTO | undefined;
  livros: LivroDTO[] = [];
  livrosEmprestados: LivroDTO[] = [];
  abrirModal: boolean = false;
  modalAnuciar: boolean = false;
  formLivro!: FormGroup;
  selectedFile: File | null = null;

  constructor(private router: Router, private formBuilder: FormBuilder, private livroService: LivroService){}
  
  async ngOnInit(): Promise<void> {
    this.buscarLivros();
    this.createFormLivro();
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

  public async deleteLivro(livroId: number): Promise<void> {
    const result = await Swal.fire({
      title: 'Tem certeza que deseja excluir?',
      text: 'Esta ação não pode ser desfeita.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, excluir',
      cancelButtonText: 'Cancelar',
      reverseButtons: true,
      focusCancel: true,
      customClass: {
        confirmButton: 'bg-red-600 hover:bg-red-700 text-white font-medium px-4 py-2 rounded-md',
        cancelButton:  'bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium px-4 py-2 rounded-md'
      },
      buttonsStyling: false
    });

    if (result.isConfirmed) {
      try {
        const mensagem = await this.livroService.delete(livroId).toPromise();

        await Swal.fire({
          title: 'Erro',
          text: 'Não foi possível excluir o livro.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
        
      } 
      catch (err: any) {
        await Swal.fire({
          title: 'Excluído!',
          text: 'O livro foi removido com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });

        this.atualizarListaDeLivros();
      }
    }
  }

  private atualizarListaDeLivros(): void {
    this.buscarLivros();
  }
  
  public abrirModalAnunciarLivro(livroId: number): void {
    
    this.livro = this.livros.find(l => l.id === livroId);

    if (!this.livro) {
      Swal.fire({
        title: 'Erro',
        text: 'Livro não encontrado.',
        icon: 'error',
        confirmButtonText: 'Fechar',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.formLivro.patchValue({
      id: this.livro.id,
      categoriaId: this.livro.categoriaId,
      quantidade: this.livro.quantidade,
      qtdPaginas: this.livro.qtdPaginas,
      valor: this.livro.valor,
      custo: this.livro.custo,
      titulo: this.livro.titulo,
      imagemUrl: this.livro.uriImagemLivro
    });

    this.abrirModal = true;
  }

  public createFormLivro(): void {
    this.formLivro = this.formBuilder.group({
      id: [0],
      categoriaId: [0],
      quantidade: [0],
      qtdPaginas: [0],
      valor: [0.00],
      custo: [0.00],
      titulo: [''],
      imagemUrl: ['']
    });
  }

  public alterarImagem() {
    throw new Error('Method not implemented.');
  }

  public fecharModal() {
    this.abrirModal = false;
    this.livro = new LivroDTO();
    this.formLivro.reset();
  }


  public async salvarEdicaoLivro() {
    if(!this.formLivro) return;

    const livroEditado = this.formLivro.value;
    const formData = new FormData();

    formData.append('Id', livroEditado.id?.toString() ?? '');
    formData.append('CategoriaId', livroEditado.categoriaId?.toString() ?? '');
    formData.append('Quantidade', livroEditado.quantidade?.toString() ?? '');
    formData.append('QtdPaginas', livroEditado.qtdPaginas?.toString() ?? '');
    formData.append('Valor', livroEditado.valor?.toString() ?? '');
    formData.append('Custo', livroEditado.custo?.toString() ?? '');
    formData.append('Titulo', livroEditado.titulo ?? '');

    if (this.selectedFile) {
      formData.append('Imagem', this.selectedFile);
    }

    (await this.livroService.update(formData)).subscribe({
      next: (livroAtualizado) => {
        Swal.fire({
          title: 'Sucesso',
          text: livroAtualizado || 'Livro atualizado com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
        this.atualizarListaDeLivros();
        this.fecharModal();
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao atualizar o livro.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
      }
    });
  }

  public onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.formLivro.get('imagemUrl')?.setValue(e.target.result); 
    };
    reader.readAsDataURL(file);
  }
}
