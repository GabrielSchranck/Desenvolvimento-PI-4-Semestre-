import { TestBed } from '@angular/core/testing';

import { CarteiraService } from './carteira.service';

describe('CarteiraService', () => {
  let service: CarteiraService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CarteiraService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
