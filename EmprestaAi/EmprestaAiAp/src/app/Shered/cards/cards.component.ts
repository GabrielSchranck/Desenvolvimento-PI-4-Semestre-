import { Component, ViewChild, ElementRef, HostListener, OnInit, OnDestroy, Input } from '@angular/core';
import { Card } from '../../core/models/Card';
import { Router } from '@angular/router';
import { LivroService } from '../../core/services/livro.service';
import { LivroAnunciadoDTO, LivroDTO } from '../../core/models/Livros';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth-service.service';
import { CarrinhoService } from '../../core/services/carrinho.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cards',
  templateUrl: './cards.component.html',
  styleUrls: ['./cards.component.css'],
  standalone: true,
  imports: [CommonModule]
})

export class CardsComponent implements OnInit {

  @ViewChild('cardContainer') cardContainer!: ElementRef;

  @Input() livrosAnunciados: LivroDTO[] = [];
  @Input() tipo: number = 0;
  @Input() titulo: string = '';

  isDragging = false;
  startX = 0;
  scrollLeft = 0;
  autoScrollInterval: any;
  isMobile = false;
  cardWidth = 320;
  showSuccess = false;

  constructor(private router: Router, private auth: AuthService, private carrinhoService: CarrinhoService) {}

  public async ngOnInit() {
    this.checkIfMobile();
    this.startAutoScroll();
    this.calculateCardWidth();
  }

  ngOnDestroy() {
    clearInterval(this.autoScrollInterval);
  }


  calculateCardWidth() {
    this.cardWidth = this.isMobile ? 280 : 320;
  }

  public goToLivro(anuncioId: number | undefined) {
    this.auth.isLoggedIn().subscribe({
      next: (isLoggedIn) => {
        if (isLoggedIn) {
          if (anuncioId !== undefined) {
            console.log('anuncioId:', anuncioId);
            console.log('tipo:', this.tipo);
            this.router.navigate(['exibelivro', anuncioId, this.tipo]);
          } else {
            console.error('anuncioId está indefinido');
          }
        } else {
          this.router.navigate(['login']);
        }
      },
      error: (error) => {
        console.error('Erro ao verificar login:', error);
        this.router.navigate(['login']);
      }
    });
  }


  public adicionarAoCarrinho(livro: LivroDTO) {
    this.auth.isLoggedIn().subscribe({
      next: (isLoggedIn) => {
        if (isLoggedIn) {
          const livroAnunciadoResult = livro.livrosAnunciados?.find(la => la.tipo === this.tipo);

          const livroAnunciado: LivroAnunciadoDTO = {
            id: livroAnunciadoResult?.id,
            clienteId: livroAnunciadoResult?.clienteId,
            tipo: livroAnunciadoResult?.tipo,
            quantidadeAnunciado: livroAnunciadoResult?.quantidadeAnunciado,
          };

          if (livroAnunciado) {
            this.carrinhoService.AddLivroToCarrinho(livroAnunciado).subscribe({
              next: (response) => {
                      Swal.fire({
                        title: 'Sucesso',
                        text: 'Livro adicionado ao carrinho.',
                        icon: 'success',
                        confirmButtonText: 'OK',
                        customClass: {
                          confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
                        },
                        buttonsStyling: false
                      });
                    },
                    error: (error) => {
                      Swal.fire({
                        title: 'Erro',
                        text: error.error || 'Livro já existe no carrinho.',
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
        } else {
          this.router.navigate(['login']);
        }
      }
    });
  }

  public startAutoScroll() {
    this.autoScrollInterval = setInterval(() => {
      if (!this.isDragging) {
        const container = this.cardContainer.nativeElement;
        if (container.scrollLeft + container.offsetWidth >= container.scrollWidth - 10) {
          container.scrollTo({ left: 0, behavior: 'smooth' });
        } else {
          container.scrollBy({ left: this.cardWidth, behavior: 'smooth' });
        }
      }
    }, 30000000);
  }

  @HostListener('window:resize')
  onResize() {
    this.checkIfMobile();
    this.calculateCardWidth();
  }

  public checkIfMobile() {
    this.isMobile = window.innerWidth <= 768;
  }


  public onMouseDown(e: MouseEvent | TouchEvent) {
    this.isDragging = true;
    const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
    this.startX = clientX - this.cardContainer.nativeElement.offsetLeft;
    this.scrollLeft = this.cardContainer.nativeElement.scrollLeft;
    clearInterval(this.autoScrollInterval);
  }

  public onMouseMove(e: MouseEvent | TouchEvent) {
    if (!this.isDragging) return;
    e.preventDefault();
    const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
    const x = clientX - this.cardContainer.nativeElement.offsetLeft;
    const walk = (x - this.startX) * 2;
    this.cardContainer.nativeElement.scrollLeft = this.scrollLeft - walk;
  }

  public onMouseUp() {
    this.isDragging = false;
    this.startAutoScroll();
  }

  public scrollManual(offset: number) {
    this.cardContainer.nativeElement.scrollBy({ left: offset, behavior: 'smooth' });
  }

  public scrollRight() {
    this.cardContainer.nativeElement.scrollBy({ left: 300, behavior: 'smooth' });
  }
}
