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

    it('should load products from live server', async(
        inject(
            [AmazonDataStore],
            (service: AmazonDataStore) => {
                const asins = 'B004QRKWKQ,B0769XXGXX,B005LKB0IU';

                service.serviceData.subscribe(data => {
                    console.log('AmazonDataStore.load:', data);
                    expect(service.isError).toEqual(
                        false,
                        'An error was not expected.'
                    );
                    expect(data).toBeTruthy();
                    expect(data.length).toEqual(3);
                });
                service.loadProducts(asins);
            }
        )
    ));
});
