import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { ActivatedRoute, Router } from '@angular/router';
import { LivroService } from '../../core/services/livro.service';
import { LivroAnunciadoDTO, LivroDTO } from '../../core/models/Livros';
import { CarteiraService } from '../../core/services/carteira.service';
import { EnderecoCliente } from '../../core/models/Cliente';
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
  enderecos: EnderecoCliente[] = [];
  livrosRelacionados: LivroDTO[] = [];
  operacao: Operacao = new Operacao();
  operacoes: Operacao[] = [];
  enderecoSelecionadoId: number = 0;
  quantidadeSelecionada: number = 1;


  constructor(private route: ActivatedRoute, private router: Router, private livroService: LivroService, private carteiraService: CarteiraService, private clienteService: ClienteService, private operacoesService: OperacoesService) { }

  public ngOnInit(): void {
    window.scrollTo(0, 0);

    this.anuncioId = Number(this.route.snapshot.paramMap.get('id'));
    this.tipo = Number(this.route.snapshot.paramMap.get('tipo'));

    this.getLivroAnunciado();
    this.getSaldoCarteira();
    this.GetUserData();
  
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
      },
      error: (error) => {
        console.error("Erro ao selecionar clientes:", error);
      }
    });
  }

  public formatNumber(value: number): string {
    return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  public getSaldoCarteira(): void {
    this.carteiraService.getSaldo().subscribe({
      next: (data) => {
        this.saldo = data.result;
      },
      error: (error) => {
        console.error('Erro ao obter saldo da carteira:', error);
      }
    });
  }

  public ComprarLivro(): void {

    if (!this.livroAnunciado.livroDTO || this.livroAnunciado.livroDTO.valor === undefined || this.saldo < this.livroAnunciado.livroDTO.valor) {
      alert('Saldo insuficiente para realizar a compra.');
      return;
    }

    this.comprarLivro = true;
  }

  public FinalizarCompra(enderecoId: number): void {

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

  public getLivrosRelacionados(livroId: number): void {
    const categoriaId = this.livroAnunciado.livroDTO?.categoriaId;

    if (categoriaId !== undefined) {
      this.livroService.GetAllLivrosAnunciadosByCat(categoriaId, livroId).subscribe({
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
}
