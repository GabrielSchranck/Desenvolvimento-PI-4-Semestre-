import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivroAnunciadoDTO, LivroDTO, LivroEmprestado } from '../../core/models/Livros';
import { CategoriasDTO } from '../../core/models/Categorias';
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
  livrosEncontrados: LivroDTO[] = [];
  Categorias: CategoriasDTO[] = [];
  livrosEmprestados: LivroEmprestado[] = [];
  livrosAnunciados: LivroDTO[] = [];
  abrirModal: boolean = false;
  modalAnuciar: boolean = false;
  modalCancelarAnuncio: boolean = false;
  formLivro!: FormGroup;
  formLivroAnunciar!: FormGroup;
  selectedFile: File | null = null;
  quantidadeDisponivel: number = 0;



  constructor(private router: Router, private formBuilder: FormBuilder, private livroService: LivroService){}
  
  async ngOnInit(): Promise<void> {
    this.buscarLivros();
    this.createFormLivro();
    this.buscarCategorias();
    this.createFormLivroAnunciar();
  }

  public GoToAddLivro(){
    this.router.navigate(['/registraLivro']);
  }

  public async buscarLivros() {
    try {
      const data: LivroDTO[] = await this.livroService.getLivros();

      this.livrosEncontrados = data ?? [];

      this.livros = this.livrosEncontrados.filter(livro => (livro.quantidade ?? 0) > 0);

      this.livrosAnunciados = [];

      this.livrosEncontrados.forEach(livro => {
        if (Array.isArray(livro.livrosAnunciados) && livro.livrosAnunciados.length > 0) {
          this.livrosAnunciados.push(livro);
        }
      });

      this.livroService.GetLivrosEmprestados().subscribe({
        next: (result) => {
          this.livrosEmprestados = result.livrosEmprestados;
          
        },
        error: (error: any) => {
          console.error("Erro ao buscar livros emprestados:", error);
        }
      });

    } catch (error) {
      console.error("❌ Erro ao buscar livros:", error);
    }
  }

  public getTempoRestante(item: LivroEmprestado): string {
    if (!item.dataDevolucao) return 'Sem data';

    const agora = new Date();
    const devolucao = new Date(item.dataDevolucao);

    const diffMs = devolucao.getTime() - agora.getTime();
    const diffDias = Math.ceil(diffMs / (1000 * 60 * 60 * 24));

    if (diffDias < 0) return 'Atrasado';
    if (diffDias === 0) return 'Último dia';

    return `${diffDias} dia(s)`;
  }


  public getTipoAnuncio(tipo: number | undefined): string {
    switch (tipo) {
      case 0: return 'Venda';
      case 1: return 'Empréstimo';
      case 2: return 'Doação';
      default: return 'Tipo desconhecido';
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
  
  public abrirModalEditarLivro(livroId: number): void {
    
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
    console.log(this.livros);
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
      imagemUrl: [''],
      tipo: [0]
    });
  }

  public createFormLivroAnunciar(): void {
    this.formLivroAnunciar = this.formBuilder.group({
      id: [0],
      clienteId: [0],
      LivroId: [0],
      tipo: ['Tipo desconhecido'],
      quantidadeAnunciado: [0]
    });
  }

  public fecharModal() {
    this.abrirModal = false;
    this.livro = new LivroDTO();
    this.formLivro.reset();
    this.selectedFile = null;
    this.modalAnuciar = false;
    this.modalCancelarAnuncio = false;
  }

  public verificarQuantidade(): void {
    let controle = this.formLivro.get('quantidade');
    
    if (!controle) controle = this.formLivroAnunciar.get('quantidadeAnunciado');

    const valor = controle?.value;

    if (valor > this.quantidadeDisponivel) {
      controle?.setValue(this.quantidadeDisponivel);
    }

    if (valor < 1) {
      controle?.setValue(1);
    }
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

  public buscarCategorias(): void {
    this.livroService.GetCategoriasLivro().subscribe(
      (response) => {
        this.Categorias = response.result;
      },
      (error) => {
        console.error("Erro ao buscar categorias:", error);
      }
    );
  }

  public abrirModalAnunciar(livroId: number): void {
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

    this.quantidadeDisponivel = this.livro.quantidade || 0;

    this.modalAnuciar = true;
  }

  public anunciarLivro(): void {
    if (this.formLivro.invalid) {
      Swal.fire({
        title: 'Erro',
        text: 'Por favor, preencha todos os campos obrigatórios.',
        icon: 'error',
        confirmButtonText: 'Fechar',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    const livroAnunciado = {
      clienteId: this.livro?.clienteId,
      LivroId: this.livro?.id,
      Tipo: this.formLivro.get('tipo')?.value || 0,
      quantidadeAnunciado: this.formLivro.get('quantidade')?.value || 0
    };
    
    this.livroService.anunciaLivro(livroAnunciado).subscribe({
      next: (response) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Livro anunciado com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.fecharModal();
          this.atualizarListaDeLivros();
        });
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao anunciar o livro.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
      }
    });

    this.modalAnuciar = false;
  }

  public cancelarAnuncio(anuncioId: number|undefined) {

    let anuncioEncontrado: LivroAnunciadoDTO | undefined;

    for (const livro of this.livrosAnunciados) {
      for (const anuncio of livro.livrosAnunciados || []) {
        if (anuncio.id === anuncioId) {
          anuncioEncontrado = anuncio;
          break;
        }
      }
      if (anuncioEncontrado) break;
    }

    this.formLivroAnunciar.patchValue({
      id: anuncioEncontrado?.id,
      clienteId: anuncioEncontrado?.clienteId,
      LivroId: anuncioEncontrado?.LivroId,
      tipo: this.getTipoAnuncio(anuncioEncontrado?.tipo),
      quantidadeAnunciado: anuncioEncontrado?.quantidadeAnunciado
    });

    this.quantidadeDisponivel = anuncioEncontrado?.quantidadeAnunciado || 0;

    this.modalCancelarAnuncio = true;
  }

  public cancelarAnuncioLivro(): void {

    var anuncioEncontrado: LivroAnunciadoDTO | undefined;
    anuncioEncontrado = this.formLivroAnunciar.value;

    if (anuncioEncontrado) {
      anuncioEncontrado.tipo = this.getTipoAnuncioNumero(String(anuncioEncontrado.tipo ?? ''));
    }

    if (anuncioEncontrado) {
      this.livroService.cancelarAnincio(anuncioEncontrado).subscribe({
        next: (response) => {
          Swal.fire({
            title: 'Sucesso',
            text: 'Anúncio cancelado com sucesso.',
            icon: 'success',
            confirmButtonText: 'OK',
            customClass: {
              confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
            },
            buttonsStyling: false
          }).then(() => {
            this.fecharModal();
            this.atualizarListaDeLivros();
          });
        }
      });
    } else {
      Swal.fire({
        title: 'Erro',
        text: 'Anúncio não encontrado.',
        icon: 'error',
        confirmButtonText: 'Fechar',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
    }
  }

  public getTipoAnuncioNumero(tipo: string): number {
      switch (tipo.toLowerCase()) {
        case 'venda': return 0;
        case 'empréstimo': return 1;
        case 'doação': return 2;
        default: return -1; 
      }
  }

  public devolverLivroEmprestado(livroId: number): void {
    this.livroService.DevolverLivro(livroId).subscribe({
      next: (response) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Livro devolvido com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.atualizarListaDeLivros();
        });
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao devolver o livro.',
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
}
