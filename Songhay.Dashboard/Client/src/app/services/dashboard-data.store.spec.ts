import { TestBed, inject, async } from '@angular/core/testing';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';

import { DashboardDataStore } from './dashboard-data.store';

describe(DashboardDataStore.name, () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
            providers: [ DashboardDataStore ]
        });
    });

    it('should be instantiated', inject(
        [DashboardDataStore],
        (service: DashboardDataStore) => {
            expect(service).toBeTruthy();
        }
    ));

    it('should load app data from live server', async(
        inject(
            [DashboardDataStore],
            (service: DashboardDataStore) => {

                service.serviceData.subscribe(data => {
                    console.log('DashboardDataStore.load:', data);
                    expect(service.isError).toEqual(
                        false,
                        'An error was not expected.'
                    );
                    expect(data).toBeTruthy();
                    expect(service.assemblyInfo).toBeTruthy();
                });
                service.loadAppData();
            }
        )
    ));
});
