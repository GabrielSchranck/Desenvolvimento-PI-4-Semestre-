import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CommonModule } from '@angular/common';
import { CarteiraService } from '../../core/services/carteira.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Cartao } from '../../core/models/Cliente';
import Swal from 'sweetalert2';
import { ClienteService } from '../../core/services/cliente-service.service';

@Component({
  selector: 'app-financeiro',
  imports: [UserInfoComponent, CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './financeiro.component.html',
  styleUrl: './financeiro.component.css'
})
export class FinanceiroComponent implements OnInit {
  @ViewChild('carousel', { static: false }) carousel!: ElementRef<HTMLDivElement>;

  saldoDisponivel: number = 0;
  cartoes: any[] = [];
  modalAdicionarAberto: boolean = false;
  modalAberto: boolean = false;
  formularioCartao!:FormGroup;
  valorAdicionar:number = 0;
  modalSaqueAberto: boolean = false;
  mostrarModalSaque: boolean = false;
  valorSaque: number = 0;
  valorSacado: number = 0;

  constructor(
    private carteiraService: CarteiraService,
    private formBuilder: FormBuilder,
    private clienteService: ClienteService
  ) { }

  public async ngOnInit(): Promise<void> {
    await this.getCartoesCliente();
    this.getSaldoDisponivel();
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

  public AdicionarFundos(valor: number): void {
    this.carteiraService.addSaldo(valor); 
  }

  public getSaldoDisponivel(): void {
    this.carteiraService.getSaldo().subscribe({
      next: (saldo) => {
        this.saldoDisponivel = saldo.saldo;
        this.valorSacado = saldo.saldoSacado;
      },
      error: (error) => {
        console.error('Erro ao obter o saldo disponível:', error);
      }
    });
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
    valor = valor.replace(/\D/g, '').substring(0, 16);
    const formatado = valor.match(/.{1,4}/g)?.join(' ') || '';
    this.formularioCartao.get('numeroCartao')?.setValue(formatado, { emitEvent: false });
  }

  public formatarDataValidade(): void{
    let valor = this.formularioCartao.get('validade')?.value || '';
    valor = valor.replace(/\D/g, '').substring(0, 4);
    const formatado = valor.match(/.{1,2}/g)?.join('/') || '';
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

  public ConfirmarSaque(saldo: number): void{
    if(this.saldoDisponivel < saldo && saldo > 0){
      Swal.fire({
        icon: 'error',
        title: 'Saldo insuficiente',
        text: 'Você não tem saldo suficiente para realizar este saque.',
        confirmButtonText: 'OK'
      });

      return;
    }

    this.clienteService.sacarSaldo(saldo).subscribe({
      next: (result: any) => {
        console.log('Saque realizado com sucesso:', result);
        this.getSaldoDisponivel();
        Swal.fire({
          icon: 'success',
          title: 'Saque realizado',
          text: `Você sacou R$ ${saldo.toFixed(2)} com sucesso!`,
          confirmButtonText: 'OK'
        }).then(() => {
          this.modalSaqueAberto = false;
          this.valorSaque = 0;
        });
      },
      error: (error: any) => {
        console.error('Erro ao realizar o saque:', error);
        Swal.fire({
          icon: 'error',
          title: 'Erro no saque',
          text: 'Ocorreu um erro ao tentar realizar o saque. Tente novamente mais tarde.',
          confirmButtonText: 'OK'
        });
      }
    });
  }
}
