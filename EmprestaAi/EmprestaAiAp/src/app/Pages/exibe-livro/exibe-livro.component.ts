import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { ActivatedRoute } from '@angular/router';
import { LivroService } from '../../core/services/livro.service';
import { LivroAnunciadoDTO } from '../../core/models/Livros';

@Component({
  selector: 'app-exibe-livro',
  imports: [NavbarComponent],
  templateUrl: './exibe-livro.component.html',
  styleUrl: './exibe-livro.component.css'
})
export class ExibeLivroComponent implements OnInit {
  anuncioId!: number;
  tipo!: number;
  livroAnunciado: LivroAnunciadoDTO = new LivroAnunciadoDTO();

  constructor(private route: ActivatedRoute, private livroService: LivroService) { }

  public ngOnInit(): void {
    window.scrollTo(0, 0);

    this.anuncioId = Number(this.route.snapshot.paramMap.get('id'));
    this.tipo = Number(this.route.snapshot.paramMap.get('tipo'));

    this.getLivroAnunciado();
  }

  private getLivroAnunciado(): void {
    this.livroService.GetLivrosAnunciados(this.anuncioId, this.tipo).subscribe({
      next: (data) => {
        this.livroAnunciado = data.livroAnunciado;
      },
      error: (error) => {
        console.error('Erro ao obter livro anunciado:', error);
      }
    });
  }

  public formatNumber(value: number): string {
    return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  
}
