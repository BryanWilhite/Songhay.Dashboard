import { TestBed, inject, async } from '@angular/core/testing';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';

import { AmazonDataStore } from './amazon-data.store';

describe(AmazonDataStore.name, () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
            providers: [ AmazonDataStore ]
        });
    });

    it('should be instantiated', inject(
        [AmazonDataStore],
        (service: AmazonDataStore) => {
            expect(service).toBeTruthy();
        }
    ));

});
