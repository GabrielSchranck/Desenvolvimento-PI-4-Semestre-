import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExibeLivroComponent } from './exibe-livro.component';

describe('ExibeLivroComponent', () => {
  let component: ExibeLivroComponent;
  let fixture: ComponentFixture<ExibeLivroComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExibeLivroComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExibeLivroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
