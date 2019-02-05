import { TestBed } from '@angular/core/testing';

import { SonghayAngularCoreService } from './songhay-angular-core.service';

describe('SonghayAngularCoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SonghayAngularCoreService = TestBed.get(SonghayAngularCoreService);
    expect(service).toBeTruthy();
  });
});
