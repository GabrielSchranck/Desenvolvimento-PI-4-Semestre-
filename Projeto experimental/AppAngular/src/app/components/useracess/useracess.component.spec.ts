import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UseracessComponent } from './useracess.component';

describe('UseracessComponent', () => {
  let component: UseracessComponent;
  let fixture: ComponentFixture<UseracessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UseracessComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UseracessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
