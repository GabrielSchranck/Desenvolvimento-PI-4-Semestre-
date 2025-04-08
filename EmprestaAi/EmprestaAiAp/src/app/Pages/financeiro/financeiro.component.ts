import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { CarteiraService } from '../../core/services/carteira.service';

@Component({
  selector: 'app-financeiro',
  imports: [UserInfoComponent, CommonModule],
  templateUrl: './financeiro.component.html',
  styleUrl: './financeiro.component.css'
})
export class FinanceiroComponent implements OnInit {

  saldoDisponivel: number = 0;
  cartoes: any[] = [];

  constructor(private carteiraService: CarteiraService) { }

  public async ngOnInit(): Promise<void> {
    await this.getCartoesCliente();
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

}
