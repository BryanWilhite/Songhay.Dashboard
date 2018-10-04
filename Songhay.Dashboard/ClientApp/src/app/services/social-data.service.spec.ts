import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';

import { SocialDataService } from './social-data.service';

describe('SocialDataService', () => {
    beforeEach(() =>
        TestBed.configureTestingModule({
            imports: [HttpModule],
            providers: [
                BaseRequestOptions,
                SocialDataService,
                {
                    deps: [XHRBackend, BaseRequestOptions],
                    provide: Http,
                    useFactory: (
                        backend: XHRBackend,
                        defaultOptions: BaseRequestOptions
                    ) => new Http(backend, defaultOptions)
                }
            ]
        }));

    it('should be created', () => {
        const service: SocialDataService = TestBed.get(SocialDataService);
        expect(service).toBeTruthy();
    });
    it(`should call ${
        SocialDataService.loadTwitterItemsMethodName
    }()`, done => {
        inject([SocialDataService], (service: SocialDataService) => {
            expect(service).not.toBeNull('the expected service is not here');

            service
                .loadTwitterItems()
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

                    console.log({
                        service: SocialDataService.name,
                        method: SocialDataService.loadTwitterItemsMethodName,
                        output: response.json()
                    });

                    done();
                })
                .catch(responseOrVoid => {
                    console.warn({
                        catchResponse: true,
                        service: SocialDataService.name,
                        method: SocialDataService.loadTwitterItemsMethodName,
                        responseOrVoid
                    });

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

                    done();
                });
        })();
    });
});
