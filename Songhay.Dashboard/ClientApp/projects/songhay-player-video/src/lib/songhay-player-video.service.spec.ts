import { TestBed } from '@angular/core/testing';

import { SonghayPlayerVideoService } from './songhay-player-video.service';

describe('SonghayPlayerVideoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SonghayPlayerVideoService = TestBed.get(SonghayPlayerVideoService);
    expect(service).toBeTruthy();
  });
});
