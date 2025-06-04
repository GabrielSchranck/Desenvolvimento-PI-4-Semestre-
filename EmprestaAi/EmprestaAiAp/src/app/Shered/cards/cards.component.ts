import { Component, ViewChild, ElementRef, HostListener, OnInit, OnDestroy } from '@angular/core';
import { Card } from '../../core/models/Card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cards',
  templateUrl: './cards.component.html',
  styleUrls: ['./cards.component.css'],
  standalone: true,
  imports: []
})
export class CardsComponent implements OnInit {
  @ViewChild('cardContainer') cardContainer!: ElementRef;

  cards = Array(10).fill(0).map((_, i) => ({
    id: i + 1,
    title: `Livro ${i + 1}`,
    author: `Autor ${i + 1}`,
    description: `Esta é uma descrição mais longa para o livro ${i + 1} que demonstra como o texto será exibido no card.`,
    image: `https://picsum.photos/300/200?random=${i}`,
    price: `R$ ${(Math.random() * 50 + 20).toFixed(2).replace('.', ',')}`,
    rating: Math.floor(Math.random() * 5) + 1
  }));

  isDragging = false;
  startX = 0;
  scrollLeft = 0;
  autoScrollInterval: any;
  isMobile = false;
  cardWidth = 320;

  constructor(private router: Router) {}

  ngOnInit() {
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

  public goToLivro(){
    this.router.navigate(['/exibelivro']);
  }

  startAutoScroll() {
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

  checkIfMobile() {
    this.isMobile = window.innerWidth <= 768;
  }


  onMouseDown(e: MouseEvent | TouchEvent) {
    this.isDragging = true;
    const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
    this.startX = clientX - this.cardContainer.nativeElement.offsetLeft;
    this.scrollLeft = this.cardContainer.nativeElement.scrollLeft;
    clearInterval(this.autoScrollInterval);
  }

  onMouseMove(e: MouseEvent | TouchEvent) {
    if (!this.isDragging) return;
    e.preventDefault();
    const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
    const x = clientX - this.cardContainer.nativeElement.offsetLeft;
    const walk = (x - this.startX) * 2;
    this.cardContainer.nativeElement.scrollLeft = this.scrollLeft - walk;
  }

  onMouseUp() {
    this.isDragging = false;
    this.startAutoScroll();
  }

  scrollManual(offset: number) {
    this.cardContainer.nativeElement.scrollBy({ left: offset, behavior: 'smooth' });
  }

  generateCards(count: number) {
    this.cards = Array.from({ length: count }, (_, i) =>
      new Card(
        i + 1,
        `Livro ${i + 1}`,
        `Descrição do livro ${i + 1}`,
        `assets/livro${i % 3 + 1}.png`,
        `R$ ${(Math.random() * 50 + 20).toFixed(2)}`,
        `Autor ${i + 1}`,
        Math.floor(Math.random() * 5) + 1,
        true
      )
    );
  }

  scrollRight() {
    this.cardContainer.nativeElement.scrollBy({ left: 300, behavior: 'smooth' });
  }
}
