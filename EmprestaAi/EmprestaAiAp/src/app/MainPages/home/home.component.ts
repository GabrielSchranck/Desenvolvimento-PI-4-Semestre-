import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavbarComponent } from "../../Shered/navbar/navbar.component";
import { CardsComponent } from "../../Shered/cards/cards.component";

@Component({
  selector: 'app-home',
  imports: [NavbarComponent, CardsComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  currentSlide = 0;
  private slideInterval: any;
  dropdownOpen = false;

  ngOnInit() {
    this.startSlideShow();
  }

  ngOnDestroy() {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
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

}
