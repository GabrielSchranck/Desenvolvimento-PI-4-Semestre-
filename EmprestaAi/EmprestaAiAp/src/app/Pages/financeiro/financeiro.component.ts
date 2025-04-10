import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CommonModule } from '@angular/common';
import { CarteiraService } from '../../core/services/carteira.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Cartao } from '../../core/models/Cliente';

@Component({
  selector: 'app-financeiro',
  imports: [UserInfoComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './financeiro.component.html',
  styleUrl: './financeiro.component.css'
})
export class FinanceiroComponent implements OnInit {
  @ViewChild('carousel', { static: false }) carousel!: ElementRef<HTMLDivElement>;

  saldoDisponivel: number = 0;
  cartoes: any[] = [];
  modalAberto: boolean = false;
  formularioCartao!:FormGroup;

  constructor(
    private carteiraService: CarteiraService,
    private formBuilder: FormBuilder
  ) { }

  public async ngOnInit(): Promise<void> {
    await this.getCartoesCliente();
  }

  public async buildFormulario(): Promise<void>{
    this.formularioCartao = this.formBuilder.group({
      id:[0],
      clienteId: [0],
      numeroCartao: [''],
      nomeImpresso: [''],
      validade: [''],
      cvv: [''],
      bandeira: ['']
    });
  }

  public async getCartoesCliente(): Promise<void> {
    (await this.carteiraService.GetCartoes()).subscribe({
      next: (result) => {
        this.cartoes = result.cartoes;
        console.log('Cartões obtidos com sucesso:', this.cartoes);
      },
      error: (error) => {
        console.error('Erro ao obter os cartões:', error);
      },
    });
  }

  public async AlteraModal(): Promise<void>{
    await this.buildFormulario();

    this.modalAberto = !this.modalAberto;
  }




  scrollLeft() {
    this.carousel.nativeElement.scrollBy({ left: -300, behavior: 'smooth' });
  }

  scrollRight() {
    this.carousel.nativeElement.scrollBy({ left: 300, behavior: 'smooth' });
  }

  public async salvarCartao(): Promise<void>{
    var cartao = this.formularioCartao.value;

    console.log("cartao", cartao);

    (await this.carteiraService.CreateCartao(cartao)).subscribe({
      next:async (result: any) => {
        console.log('Cartão criado com sucesso:', result);
        await this.getCartoesCliente();
        this.formularioCartao.reset();
        this.modalAberto = false;
      },
      error:(error: any) => {
        console.error('Erro ao criar o cartão:', error);
      },
    });

    this.AlteraModal();
  }

  public formatarNumeroCartao(): void{
    let valor = this.formularioCartao.get('numeroCartao')?.value || '';

    console.log("teste")

    // Remove tudo que não for número
    valor = valor.replace(/\D/g, '').substring(0, 16); // limita a 16 dígitos

    // Adiciona espaço a cada 4 dígitos
    const formatado = valor.match(/.{1,4}/g)?.join(' ') || '';

    // Atualiza o valor sem emitir evento (pra não causar loop ou trigger de validação)
    this.formularioCartao.get('numeroCartao')?.setValue(formatado, { emitEvent: false });
  }

  public formatarDataValidade(): void{
    let valor = this.formularioCartao.get('validade')?.value || '';

    // Remove tudo que não for número
    valor = valor.replace(/\D/g, '').substring(0, 4); // limita a 4 dígitos

    // Adiciona espaço a cada 2 dígitos
    const formatado = valor.match(/.{1,2}/g)?.join('/') || '';

    // Atualiza o valor sem emitir evento (pra não causar loop ou trigger de validação)
    this.formularioCartao.get('validade')?.setValue(formatado, { emitEvent: false });
  }

  public async excluirCartao(cartao: Cartao): Promise<void> {
    (await this.carteiraService.DeleteCartao(cartao)).subscribe({
      next: async (result) => {
        console.log('Cartão excluído com sucesso:', result);
        await this.getCartoesCliente();
      },
      error: (error) => {
        console.error('Erro ao excluir o cartão:', error);
      }
    });
  }

}
