import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { CardsComponent } from "../../Shered/cards/cards.component";
import { LivroDTO } from '../../core/models/Livros';
import { LivroService } from '../../core/services/livro.service';
import { CategoriasDTO } from '../../core/models/Categorias';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [NavbarComponent, CardsComponent, CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {


  currentSlide = 0;
  private slideInterval: any;
  dropdownOpen = false;
  searchTerm: string = '';

  livrosAnunciados: LivroDTO[] = [];
  livrosAnunciadosFiltrado: LivroDTO[] = [];

  livrosVenda: LivroDTO[] = [];
  livrosEmprestimo: LivroDTO[] = [];
  livrosDoacao: LivroDTO[] = [];

  livrosVendaFiltrado: LivroDTO[] = [];
  livrosEmprestimoFiltrado: LivroDTO[] = [];
  livrosDoacaoFiltrado: LivroDTO[] = [];

  categorias: CategoriasDTO[] = [];
  possuiFiltroAtivo = false;
  isTyping = false;

  constructor(private livroServices: LivroService) {}

  public ngOnInit() {
    this.startSlideShow();
    this.initAsync();
  }

  private async initAsync() {
    await this.loadCategorias();
    await this.loadLivrosAnunciados();
  }

  public ngOnDestroy() {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
    }
  }

  private async loadLivrosAnunciados(): Promise<void> {
    this.livroServices.GetAllLivrosAnunciados().subscribe({
      next: (livros) => {
        this.livrosAnunciados = livros.livrosAnunciados;
        this.livrosVenda = livros.livrosVendidos;
        this.livrosEmprestimo = livros.livrosEmprestimos;
        this.livrosDoacao = livros.livrosDoacoes;
      } ,
      error: (error) => {
        console.error("Erro ao carregar livros anunciados:", error);
      }   
    });
  }

  public filterLivros() {
    const termo = this.searchTerm.toLowerCase().trim();
    
    if(termo === ''){
      this.isTyping = false;
    }
    else {
      this.isTyping = true;
    }

    if (termo) {
      this.livrosAnunciadosFiltrado = this.livrosAnunciados.filter(livro =>
        livro.titulo && livro.titulo.toLowerCase().includes(termo)
      );
    }
  }

  private startSlideShow() {
    this.slideInterval = setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % 4;
    }, 5000);
  }

  public toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  private async loadCategorias(): Promise<void> {
    this.livroServices.GetCategoriasLivro().subscribe({
      next: (categorias) => {
        this.categorias = categorias.result;
      },
      error: (error) => {
        console.error("Erro ao carregar categorias:", error);
      }
    });
  }

  public selectCategory(categoriaId: number|undefined) {
    this.possuiFiltroAtivo = true;
    this.dropdownOpen = false;

    this.livrosVendaFiltrado = this.livrosVenda.filter(livro => livro.categoriaId === categoriaId);
    this.livrosEmprestimoFiltrado = this.livrosEmprestimo.filter(livro => livro.categoriaId === categoriaId);
    this.livrosDoacaoFiltrado = this.livrosDoacao.filter(livro => livro.categoriaId === categoriaId);

  }

  public toggleFiltro() {
    this.possuiFiltroAtivo = false;
    this.dropdownOpen = false;
    this.isTyping = false;

    this.livrosVendaFiltrado = [];
    this.livrosEmprestimoFiltrado = [];
    this.livrosDoacaoFiltrado = [];
    this.livrosAnunciadosFiltrado = [];

    this.searchTerm = '';
  }
}
