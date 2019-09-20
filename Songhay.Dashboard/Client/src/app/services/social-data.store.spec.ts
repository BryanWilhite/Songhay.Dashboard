import { TestBed, inject, async } from '@angular/core/testing';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';

import { SocialDataStore } from './social-data.store';

describe(SocialDataStore.name, () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
            providers: [ SocialDataStore ]
        });
    });

    it('should be instantiated', inject(
        [SocialDataStore],
        (service: SocialDataStore) => {
            expect(service).toBeTruthy();
        }
    ));

    it('should load Twitter items from live server', async(
        inject(
            [SocialDataStore],
            (service: SocialDataStore) => {

                service.serviceData.subscribe(data => {
                    // console.log('SocialDataStore.loadTwitterItems:', data);
                    expect(service.isError).toEqual(
                        false,
                        'An error was not expected.'
                    );
                    expect(data).toBeTruthy();
                });
                service.loadTwitterItems();
            }
        )
    ));
});
