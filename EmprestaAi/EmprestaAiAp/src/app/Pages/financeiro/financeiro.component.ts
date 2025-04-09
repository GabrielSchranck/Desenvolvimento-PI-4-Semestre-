import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CommonModule } from '@angular/common';
import { CarteiraService } from '../../core/services/carteira.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-financeiro',
  imports: [UserInfoComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './financeiro.component.html',
  styleUrl: './financeiro.component.css'
})
export class FinanceiroComponent implements OnInit {

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
    await this.buildFormulario();
  }

  public async buildFormulario(): Promise<void>{
    this.formularioCartao = this.formBuilder.group({
      id:[''],
      clienteId: [''],
      numeroCartao: [''],
      numeroImpresso: [''],
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

  public AlteraModal(): void{
    this.modalAberto = !this.modalAberto;
    console.log(this.modalAberto)
  }

  public async salvarCartao(): Promise<void>{

  }

  public formatarCartao(): void{
    let valor = this.formularioCartao.get('numeroCartao')?.value || '';

    console.log("teste")

    // Remove tudo que não for número
    valor = valor.replace(/\D/g, '').substring(0, 16); // limita a 16 dígitos

    // Adiciona espaço a cada 4 dígitos
    const formatado = valor.match(/.{1,4}/g)?.join(' ') || '';

    // Atualiza o valor sem emitir evento (pra não causar loop ou trigger de validação)
    this.formularioCartao.get('numeroCartao')?.setValue(formatado, { emitEvent: false });
  }
}
