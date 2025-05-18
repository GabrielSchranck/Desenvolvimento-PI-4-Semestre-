import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CriaLivroComponent } from './cria-livro.component';

describe('CriaLivroComponent', () => {
  let component: CriaLivroComponent;
  let fixture: ComponentFixture<CriaLivroComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CriaLivroComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CriaLivroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
