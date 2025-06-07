import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CarrinhoService } from '../../core/services/carrinho.service';
import { CarrinhoDTO, ItemCarrinhoDTO } from '../../core/models/Carrinho';

@Component({
  selector: 'app-carrinho',
  imports: [UserInfoComponent],
  templateUrl: './carrinho.component.html',
  styleUrl: './carrinho.component.css'
})
export class CarrinhoComponent implements OnInit {
  
  
  carrinho: CarrinhoDTO = new CarrinhoDTO();
  itensVenda: ItemCarrinhoDTO[] = [];
  itensEmprestimo: ItemCarrinhoDTO[] = [];
  itensDoacao: ItemCarrinhoDTO[] = [];

  mostrarTodosVenda: boolean = false;
  mostrarTodosEmprestimo: boolean = false;
  mostrarTodosDoacao: boolean = false;
  carrinhoExistente: boolean = false;
  possuiItens: boolean = false;
  possuiItensVenda: boolean = false;
  possuiItensEmprestimo: boolean = false;
  possuiItensDoacao: boolean = false;


  constructor(private carrinhoService: CarrinhoService) { }

  public ngOnInit(): void {
    this.VerificarCarrinhoExistente();
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

  private VerificarCarrinhoExistente(): void {
    this.carrinhoService.VerificarCarrinhoExistente()
      .then((existe: { result: boolean }) => {
        this.carrinhoExistente = existe.result;
        if (!this.carrinhoExistente) {
          this.createCarrinho();
        } else {

          this.GetCarrinho();
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

}
