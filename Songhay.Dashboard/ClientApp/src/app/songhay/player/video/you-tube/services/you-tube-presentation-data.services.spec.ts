import { TestBed, inject } from '@angular/core/testing';

import { YouTubePresentationDataServices } from './you-tube-presentation-data.services';

describe('YouTubePresentationDataService', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [YouTubePresentationDataServices]
        });
    });

    it('should be created', inject(
        [YouTubePresentationDataServices],
        (service: YouTubePresentationDataServices) => {
            expect(service).toBeTruthy();
        }
    ));
});
