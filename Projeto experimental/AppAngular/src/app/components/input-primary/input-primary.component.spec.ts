import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputPrimaryComponent } from './input-primary.component';

describe('InputPrimaryComponent', () => {
  let component: InputPrimaryComponent;
  let fixture: ComponentFixture<InputPrimaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InputPrimaryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InputPrimaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
