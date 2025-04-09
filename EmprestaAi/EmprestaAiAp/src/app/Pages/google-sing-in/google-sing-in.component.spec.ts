import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoogleSingInComponent } from './google-sing-in.component';

describe('GoogleSingInComponent', () => {
  let component: GoogleSingInComponent;
  let fixture: ComponentFixture<GoogleSingInComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GoogleSingInComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GoogleSingInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
