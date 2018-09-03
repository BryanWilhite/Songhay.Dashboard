import { TestBed, inject } from '@angular/core/testing';

import { YouTubeDataService } from './you-tube-data.service';

describe('YouTubeDataService', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [YouTubeDataService]
        });
    });

    it('should be created', inject(
        [YouTubeDataService],
        (service: YouTubeDataService) => {
            expect(service).toBeTruthy();
        }
    ));
});
