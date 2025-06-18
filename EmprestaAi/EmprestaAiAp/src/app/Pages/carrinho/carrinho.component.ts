import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CarrinhoService } from '../../core/services/carrinho.service';
import { CarrinhoDTO, ItemCarrinhoDTO } from '../../core/models/Carrinho';
import Swal from 'sweetalert2';
import { Operacao } from '../../core/models/Operacao';
import { OperacoesService } from '../../core/services/operacoes.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Endereco } from '../../core/models/Endereco';
import { EnderecoService } from '../../core/services/endereco.service';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Cliente } from '../../core/models/Cliente';

@Component({
  selector: 'app-carrinho',
  imports: [UserInfoComponent, CommonModule, FormsModule],
  templateUrl: './carrinho.component.html',
  styleUrl: './carrinho.component.css'
})
export class CarrinhoComponent implements OnInit {
  
  
  carrinho: CarrinhoDTO = new CarrinhoDTO();
  itensVenda: ItemCarrinhoDTO[] = [];
  itensEmprestimo: ItemCarrinhoDTO[] = [];
  itensDoacao: ItemCarrinhoDTO[] = [];
  itensSolicitados: ItemCarrinhoDTO[] = [];
  operacao!: Operacao;
  operacoes: Operacao[] = [];
  enderecos: Endereco[] = [];
  cliente!: Cliente;
  somaValorVenda: number = 0;
  quantidadeSelecionada: number = 1;
  enderecoSelecionadoId: number = 0;
  quantidadesSelecionadas: { [id: number]: number } = {};

  mostrarTodosVenda: boolean = false;
  mostrarTodosEmprestimo: boolean = false;
  mostrarTodosDoacao: boolean = false;
  carrinhoExistente: boolean = false;
  possuiItens: boolean = false;
  possuiItensVenda: boolean = false;
  possuiItensEmprestimo: boolean = false;
  possuiItensDoacao: boolean = false;
  comprarLivro:boolean = false;
  mostrarModalSolicitacao: boolean = false;


  constructor(private carrinhoService: CarrinhoService, private operacoesServices: OperacoesService, private clienteService: ClienteService) { }

  public ngOnInit(): void {
    this.VerificarCarrinhoExistente();
  }

  public formatNumber(value: number): string {
    return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  get itensVendaVisiveis() {
    return this.mostrarTodosVenda ? this.itensVenda : this.itensVenda.slice(0, this.itensVenda.length);
  }

  get itensEmprestimoVisiveis() {
    return this.mostrarTodosEmprestimo ? this.itensEmprestimo : this.itensEmprestimo.slice(0, this.itensEmprestimo.length);
  }

  get itensDoacaoVisiveis() {
    return this.mostrarTodosDoacao ? this.itensDoacao : this.itensDoacao.slice(0, this.itensDoacao.length);
  }

  public calcularSomaValorVenda(): number {
    this.somaValorVenda = this.itensVenda.reduce((total, item) => {
      const id = item.id!;
      const quantidade = this.quantidadesSelecionadas[id] ?? 1;
      const valorLivro = item.livroAnunciadoDTO?.livroDTO?.valor ?? 0;
      const taxa = item.livroAnunciadoDTO?.valorTaxa ?? 0;
      return total + (valorLivro + taxa) * quantidade;
    }, 0);
    return this.somaValorVenda;
  }

  public calcularTaxaServico(): number {
    return this.itensVenda.reduce((total, item) => {
      const id = item.id!;
      const quantidade = this.quantidadesSelecionadas[id] ?? 1;
      const taxaUnitaria = item.livroAnunciadoDTO?.valorTaxa ?? 0;
      return total + taxaUnitaria * quantidade;
    }, 0);
  }

  public calcularTotalComTaxa(): number {
    const subtotal = this.itensVenda.reduce((total, item) => {
      const id = item.id!;
      const quantidade = this.quantidadesSelecionadas[id] ?? 1;
      const valorLivro = item.livroAnunciadoDTO?.livroDTO?.valor ?? 0;
      return total + valorLivro * quantidade;
    }, 0);

    const taxaTotal = this.calcularTaxaServico();

    return subtotal + taxaTotal;
  }

  private VerificarCarrinhoExistente(): void {
    this.carrinhoService.VerificarCarrinhoExistente()
      .then((existe: { result: boolean }) => {
        this.carrinhoExistente = existe.result;
        if (!this.carrinhoExistente) {
          this.createCarrinho();
        } else {
          this.GetCarrinho();
          this.carregarEnderecos();
        }
      })
      .catch((error: any) => {
        console.error('Erro ao verificar carrinho existente:', error);
      });
  }

  private async createCarrinho(): Promise<void> {
    try {
      await this.carrinhoService.Create();
    } catch (error) {
      console.error('Erro ao criar carrinho:', error);
    }
  }

  private GetCarrinho(): void {
    this.carrinhoService.GetCarrinho().subscribe(
      (carrinho: { result: CarrinhoDTO; }) => {
        this.carrinho = carrinho.result;
        this.itensVenda = this.carrinho.itens.filter(item => item.livroAnunciadoDTO?.tipo === 0);
        this.itensEmprestimo = this.carrinho.itens.filter(item => item.livroAnunciadoDTO?.tipo === 1);
        this.itensDoacao = this.carrinho.itens.filter(item => item.livroAnunciadoDTO?.tipo === 2);

        if(this.carrinho.itens.length > 0) {
          this.possuiItens = true;
        }

        if(this.itensVenda.length > 0) {
          this.possuiItensVenda = true;
        }

        if(this.itensEmprestimo.length > 0) {
          this.possuiItensEmprestimo = true;
        }

        if(this.itensDoacao.length > 0) {
          this.possuiItensDoacao = true;
        }
      },
      (error: any) => {
        console.error('Erro ao obter carrinho:', error);
      }
    );
  }

  public RemoverItemCarrinho(itemId: number): void {
    this.carrinhoService.RemoveItemCarrinho(itemId)
      .subscribe(
        (result) => {
          Swal.fire({
            title: 'Sucesso',
            text: result.result,
            icon: 'success',
            confirmButtonText: 'OK',
            customClass: {
              confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
            },
            buttonsStyling: false
          }).then(() => {
            //this.VerificarCarrinhoExistente()
            window.location.reload();
          });
        },
        (error: any) => {
          Swal.fire({
            title: 'Erro',
            text: error.error || 'Erro ao remover item do carrinho.',
            icon: 'error',
            confirmButtonText: 'Fechar',
            customClass: {
              confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
            },
            buttonsStyling: false
          });
        }
      );
  }

  public FinalizarCompra(): void {
    if (this.carrinho.itens.length === 0) {
      Swal.fire({
        title: 'Carrinho vazio',
        text: 'Adicione itens ao carrinho antes de finalizar a compra.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    const valorCompra = this.calcularTotalComTaxa();

    if ((this.cliente?.saldo ?? 0) < valorCompra) {
      Swal.fire({
        title: 'Saldo insuficiente',
        text: 'Você não possui saldo suficiente para finalizar a compra.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    if(this.enderecoSelecionadoId === 0) {
      Swal.fire({
        title: 'Endereço não selecionado',
        text: 'Por favor, selecione um endereço para entrega.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.itensVenda.forEach(item => {
      const operacao = new Operacao();
      if (item.livroAnunciadoDTO) {
        operacao.LivroAnunciadoDTO = item.livroAnunciadoDTO;
        operacao.tipo = item.livroAnunciadoDTO.tipo || 0; 
        operacao.Quantidade = item.quantidade || 1; 
        this.operacoes.push(operacao);
      }
    });

    this.operacoesServices.ComprarLivro(this.operacoes).subscribe({
      next: (response) => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Compra finalizada com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.GetCarrinho();
        });
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao finalizar a compra.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
      }
    });

    this.itensVenda.forEach(item => {
      this.RemoverItemCarrinho(item.id!);
    })
  }

  public SolicitarEmprestimo(): void {
    if (this.carrinho.itens.length === 0) {
      Swal.fire({
        title: 'Carrinho vazio',
        text: 'Adicione itens ao carrinho antes de finalizar o empréstimo.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    if(this.enderecoSelecionadoId === 0) {
      Swal.fire({
        title: 'Endereço não selecionado',
        text: 'Por favor, selecione um endereço para entrega.',
        icon: 'warning',
        confirmButtonText: 'OK',
        customClass: {
          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
        },
        buttonsStyling: false
      });
      return;
    }

    this.itensSolicitados.forEach(item => {
      const operacao = new Operacao();
      if (item.livroAnunciadoDTO) {
        operacao.LivroAnunciadoDTO = item.livroAnunciadoDTO;
        operacao.tipo = item.livroAnunciadoDTO.tipo || 0; 
        operacao.Quantidade = 1; 
        this.operacoes.push(operacao);
      }
    });

    this.operacoesServices.SolicitarEmprestimoDoacao(this.operacoes).subscribe({
      next: () => {
        Swal.fire({
          title: 'Sucesso',
          text: 'Empréstimo finalizado com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        }).then(() => {
          this.GetCarrinho();
        });
      },
      error: (error: { error: any; }) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao finalizar o empréstimo.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
      }
    });

    this.itensSolicitados.forEach(item => {
      this.RemoverItemCarrinho(item.id!);
    })
  }

  private carregarEnderecos(): void {
    this.clienteService.GetByToken().subscribe({
      next: (cliente) => {
        this.enderecos = cliente.enderecos;
        this.cliente = cliente.cliente;
      },
      error: (error) => {
        Swal.fire({
          title: 'Erro',
          text: error.error || 'Erro ao carregar endereços.',
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

  public solicitarLivros(tipo: number): void {
    if(tipo === 1){
      this.itensSolicitados = this.itensEmprestimo;
    }

    else if(tipo === 2){
      this.itensSolicitados = this.itensDoacao;
    }

    this.mostrarModalSolicitacao = true;
  }

  public cancelarSolicitacao(): void {
    this.mostrarModalSolicitacao = false;
    this.itensSolicitados = [];
  }
}
