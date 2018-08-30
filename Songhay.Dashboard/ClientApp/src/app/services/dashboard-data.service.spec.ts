import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';
import { AppScalars } from '../models/songhay-app-scalars';
import { DashboardDataService } from './dashboard-data.service';

describe('DashboardDataService', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                BaseRequestOptions,
                DashboardDataService,
                {
                    deps: [XHRBackend, BaseRequestOptions],
                    provide: Http,
                    useFactory: (
                        backend: XHRBackend,
                        defaultOptions: BaseRequestOptions
                    ) => new Http(backend, defaultOptions)
                }
            ],
            imports: [HttpModule]
        });
    });

    it('should be here', inject(
        [DashboardDataService],
        (service: DashboardDataService) => {
            expect(service).toBeTruthy();
        }
    ));
    it('should load App data', done => {
        inject([DashboardDataService], (service: DashboardDataService) => {
            AppScalars.appDataLocation = './base/src/assets/data/app.json';

            expect(service).not.toBeNull('the expected service is not here');

            service
                .loadAppData()
                .then(responseOrVoid => {
                    const response = responseOrVoid as Response;
                    expect(response).toBeDefined(
                        'The expected response is not defined.'
                    );
                    expect(response).not.toBeNull(
                        'The expected response is not here.'
                    );
                    expect(response.ok).toBe(
                        true,
                        'The expected OK response is not here.'
                    );

                    expect(service.isError).toEqual(
                        false,
                        'Service in error state is unexpected.'
                    );
                    expect(service.isLoaded).toEqual(
                        true,
                        'The expected Service loaded state is not here.'
                    );
                    expect(service.isLoading).toEqual(
                        false,
                        'The expected Service loading state is not here.'
                    );

                    expect(service.feeds).not.toBeNull(
                        'The expected Service feeds are not here.'
                    );
                    expect(service.assemblyInfo).not.toBeNull(
                        'The expected Assembly Info is not here.'
                    );

                    console.log({ assemblyInfo: service.assemblyInfo });
                    console.log({ 'service.feeds': service.feeds });

                    done();
                })
                .catch(response => {
                    console.log(
                        `${
                            DashboardDataService.loadFeedsMethodName
                        }() catch response: `,
                        response
                    );

                    done();
                });
        })();
    });
});
