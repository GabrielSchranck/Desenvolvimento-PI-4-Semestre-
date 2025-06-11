import { TestBed } from '@angular/core/testing';

import { OperacoesService } from './operacoes.service';

describe('OperacoesService', () => {
  let service: OperacoesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OperacoesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
