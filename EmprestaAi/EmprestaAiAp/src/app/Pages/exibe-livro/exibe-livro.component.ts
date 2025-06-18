import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { ActivatedRoute, Router } from '@angular/router';
import { LivroService } from '../../core/services/livro.service';
import { ComentarioLivroDTO, LivroAnunciadoDTO, LivroDTO } from '../../core/models/Livros';
import { CarteiraService } from '../../core/services/carteira.service';
import { Cliente, EnderecoCliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente-service.service';
import { OperacoesService } from '../../core/services/operacoes.service';
import { Operacao } from '../../core/models/Operacao';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { CardsComponent } from "../../Shered/cards/cards.component";

@Component({
  selector: 'app-exibe-livro',
  imports: [NavbarComponent, CommonModule, FormsModule, CardsComponent],
  templateUrl: './exibe-livro.component.html',
  styleUrl: './exibe-livro.component.css'
})
export class ExibeLivroComponent implements OnInit {
  anuncioId!: number;
  tipo!: number;
  livroAnunciado: LivroAnunciadoDTO = new LivroAnunciadoDTO();
  saldo: number = 0;
  comprarLivro: boolean = false;
  emprestimoDocao: boolean = false;
  enderecos: EnderecoCliente[] = [];
  livrosRelacionados: LivroDTO[] = [];
  operacao: Operacao = new Operacao();
  operacoes: Operacao[] = [];
  enderecoSelecionadoId: number = 0;
  quantidadeSelecionada: number = 1;
  quantidadeEmprestimo: number = 1;
  comentarios: ComentarioLivroDTO[] = [];
  comentario: string = '';
  cliente!: Cliente;
  comentarioOriginal: string = '';


  constructor(private route: ActivatedRoute, private router: Router, private livroService: LivroService, private carteiraService: CarteiraService, private clienteService: ClienteService, private operacoesService: OperacoesService) { }

  public ngOnInit(): void {
    window.scrollTo(0, 0);

    this.anuncioId = Number(this.route.snapshot.paramMap.get('id'));
    this.tipo = Number(this.route.snapshot.paramMap.get('tipo'));

    this.getLivroAnunciado();
    this.getSaldoCarteira();
    this.GetUserData();
    this.getComentarios(this.anuncioId);

  }

  private getLivroAnunciado(): void {
    this.livroService.GetLivrosAnunciados(this.anuncioId, this.tipo).subscribe({
      next: (data) => {
        this.livroAnunciado = data.livroAnunciado;
        
        if (this.livroAnunciado.livroDTO?.id !== undefined) {
          this.getLivrosRelacionados(this.livroAnunciado.livroDTO.id);
        }
      },
      error: (error) => {
        console.error('Erro ao obter livro anunciado:', error);
      }
    });
  }

  private GetUserData(): void {
    this.clienteService.GetByToken().subscribe({
      next: (dados) => {
        this.enderecos = dados.enderecos;
        this.cliente = dados.cliente;
      },
      error: (error) => {
        console.error("Erro ao selecionar clientes:", error);
      }
    });
  }

  public excluirComentario(comentarioId: number|undefined) {
    if (!comentarioId) {
      Swal.fire({
        title: 'Erro',
        text: 'Comentário não encontrado.',
        icon: 'error',
        confirmButtonText: 'Fechar',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    Swal.fire({
      title: 'Excluir comentário',
      text: 'Você tem certeza que deseja excluir este comentário?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, excluir',
      cancelButtonText: 'Cancelar',
      customClass: {
        confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md',
        cancelButton: 'bg-gray-300 hover:bg-gray-400 text-black font-medium px-4 py-2 rounded-md'
      },
      buttonsStyling: false
    }).then((result) => {
      if (result.isConfirmed) {
        this.livroService.DeletarComentarioLivro(comentarioId).subscribe({
          next: (data) => {
            Swal.fire({
              title: 'Sucesso',
              text: 'Comentário excluído com sucesso.',
              icon: 'success',
              confirmButtonText: 'OK',
              customClass: {
                confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
              },
              buttonsStyling: false
            }).then(() => {
              this.getComentarios(this.livroAnunciado.livroDTO?.id || 0);
            });
          },
          error: (error) => {
            Swal.fire({
              title: 'Erro',
              text: error.error || 'Erro ao excluir comentário.',
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
    });
  }

  public editarComentario(comentario: ComentarioLivroDTO) {

    if(this.cliente.id === comentario.clienteId) {
      this.comentarioOriginal = this.comentario;
      comentario.editar = true;
    }
  }

  public salvarComentario(ComentarioLivroDTO: ComentarioLivroDTO): void{
    if (!ComentarioLivroDTO.comentario || ComentarioLivroDTO.comentario.trim() === '') {
      Swal.fire({
        title: 'Comentário vazio',
        text: 'Por favor, insira um comentário antes de salvar.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.livroService.EditarComentarioLivro(ComentarioLivroDTO).subscribe({
      next: (data) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Comentário editado com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.getComentarios(this.livroAnunciado.livroDTO?.id || 0);
        });
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao editar comentário.',
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

  public formatNumber(value: number): string {
    return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  public getSaldoCarteira(): void {
    this.carteiraService.getSaldo().subscribe({
      next: (data) => {
        this.saldo = data.saldo;
      },
      error: (error) => {
        console.error('Erro ao obter saldo da carteira:', error);
      }
    });
  }

  public cancelarEdicao(item: any) {
    this.comentario = this.comentarioOriginal;
    item.editar = false;
  }

  public ComprarLivro(): void {

    if (!this.livroAnunciado.livroDTO || this.livroAnunciado.livroDTO.valor === undefined || this.saldo < this.livroAnunciado.livroDTO.valor) {
      alert('Saldo insuficiente para realizar a compra.');
      return;
    }

    this.comprarLivro = true;
  }

  public FinalizarCompra(enderecoId: number): void {

    if (!enderecoId) {
      Swal.fire({
        title: 'Endereço não selecionado',
        text: 'Por favor, selecione um endereço para prosseguir com a compra.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.operacao.LivroAnunciadoDTO = this.livroAnunciado;
    this.operacao.enderecoId = enderecoId;
    this.operacao.tipo = this.tipo;
    this.operacao.Quantidade = this.quantidadeSelecionada;

    this.operacoes.push(this.operacao);

    this.operacoesService.ComprarLivro(this.operacoes).subscribe({
      next: (response) => {
              Swal.fire({
                title: 'Sucesso',
                text: 'Livro comprado com sucesso.',
                icon: 'success',
                confirmButtonText: 'OK',
                customClass: {
                  confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
                },
                buttonsStyling: false
              }).then(() => {
                this.comprarLivro = false;
                this.router.navigate(['/livros']);
              });
            },
            error: (error) => {
              Swal.fire({
                title: 'Erro',
                text: error.error || 'Erro ao comprar o livro.',
                icon: 'error',
                confirmButtonText: 'Fechar',
                customClass: {
                  confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
                },
                buttonsStyling: false
              }).then(() => {
                this.comprarLivro = false;
              });
            }
          });
  }

  public ConfirmarEmprestimo(enderecoId: number): void {
    if (!enderecoId) {
      Swal.fire({
        title: 'Endereço não selecionado',
        text: 'Por favor, selecione um endereço para prosseguir com a compra.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.operacao.LivroAnunciadoDTO = this.livroAnunciado;
    this.operacao.enderecoId = enderecoId;
    this.operacao.tipo = this.tipo;
    this.operacao.Quantidade = this.quantidadeSelecionada;

    this.operacoes.push(this.operacao);

    this.operacoesService.SolicitarEmprestimoDoacao(this.operacoes).subscribe({
      next: (response) => {
              Swal.fire({
                title: 'Sucesso',
                text: 'Livro solicitado.',
                icon: 'success',
                confirmButtonText: 'OK',
                customClass: {
                  confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
                },
                buttonsStyling: false
              }).then(() => {
                this.comprarLivro = false;
                this.router.navigate(['/livros']);
              });
            },
            error: (error) => {
              Swal.fire({
                title: 'Erro',
                text: error.error || 'Erro ao solicitar.',
                icon: 'error',
                confirmButtonText: 'Fechar',
                customClass: {
                  confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
                },
                buttonsStyling: false
              }).then(() => {
                this.comprarLivro = false;
              });
            }
          });
  }

  public getLivrosRelacionados(livroId: number): void {
    const categoriaId = this.livroAnunciado.livroDTO?.categoriaId;

    if (categoriaId !== undefined) {
      this.livroService.GetAllLivrosAnunciadosByCat(categoriaId, livroId, this.tipo).subscribe({
        next: (data) => {
          this.livrosRelacionados = data.livrosAnunciados;
          
        },
        error: (error) => {
          console.error('Erro ao obter livros relacionados:', error);
        }
      });
    } else {
      console.warn('categoriaId está indefinido, não foi possível buscar livros relacionados.');
    }
  }

  public getComentarios(livroId: number): void {
    this.livroService.GetComentariosLivro(livroId).subscribe({
      next: (data) => {
        this.comentarios = data.comentarios;
      },
      error: (error) => {
        console.error('Erro ao obter comentários:', error);
      }
    });
  }

  public EnviarComentario(): void {
    const comentario: ComentarioLivroDTO = {
      livroId: this.livroAnunciado.livroDTO?.id || 0,
      comentario: this.comentario.trim(),
    }

    if(!this.comentario || this.comentario.trim() === '') {
      Swal.fire({
        title: 'Comentário vazio',
        text: 'Por favor, insira um comentário antes de enviar.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.livroService.AdicionarComentarioLivro(comentario).subscribe({
      next: (data) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Comentário enviado com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.comentario = '';
          this.getComentarios(this.livroAnunciado.livroDTO?.id || 0);
        });
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao enviar comentário.',
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

  public CalculaDiasQueComentou(dataComentario: Date): number {
    const dataAtual = new Date();
    const dataComentarioObj = new Date(dataComentario);
    const diffTime = Math.abs(dataAtual.getTime() - dataComentarioObj.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  }
}
