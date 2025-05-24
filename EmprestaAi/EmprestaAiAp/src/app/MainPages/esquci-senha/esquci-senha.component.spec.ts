import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EsquciSenhaComponent } from './esquci-senha.component';

describe('EsquciSenhaComponent', () => {
  let component: EsquciSenhaComponent;
  let fixture: ComponentFixture<EsquciSenhaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EsquciSenhaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EsquciSenhaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
