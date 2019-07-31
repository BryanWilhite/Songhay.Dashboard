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

});
