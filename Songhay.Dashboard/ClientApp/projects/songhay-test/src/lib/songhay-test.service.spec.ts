import { TestBed } from '@angular/core/testing';

import { SonghayTestService } from './songhay-test.service';

describe('SonghayTestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SonghayTestService = TestBed.get(SonghayTestService);
    expect(service).toBeTruthy();
  });
});
