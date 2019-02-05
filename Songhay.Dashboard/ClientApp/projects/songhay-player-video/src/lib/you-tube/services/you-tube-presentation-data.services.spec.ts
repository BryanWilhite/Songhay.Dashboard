import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';

import { YouTubePresentationDataServices } from './you-tube-presentation-data.services';

describe(YouTubePresentationDataServices.name, () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpModule],
            providers: [
                BaseRequestOptions,
                YouTubePresentationDataServices,
                {
                    deps: [XHRBackend, BaseRequestOptions],
                    provide: Http,
                    useFactory: (
                        backend: XHRBackend,
                        defaultOptions: BaseRequestOptions
                    ) => new Http(backend, defaultOptions)
                }
            ]
        });
    });

    it('should be created', inject(
        [YouTubePresentationDataServices],
        (service: YouTubePresentationDataServices) => {
            expect(service).toBeTruthy();
        }
    ));

    it(`should call ${
        YouTubePresentationDataServices.loadPresentationMethodName
    }()`, done => {
        inject(
            [YouTubePresentationDataServices],
            (services: YouTubePresentationDataServices) => {
                expect(services).not.toBeNull(
                    'the expected service is not here'
                );

                const id = 'bowie0';

                services
                    .loadPresentation(id)
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

                        const service = services.presentationDataService;

                        expect(service).toBeDefined(
                            'The expected service is not defined.'
                        );
                        expect(service).not.toBeNull(
                            'The expected service is not here.'
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

                        console.log({
                            serviceName: YouTubePresentationDataServices.name,
                            service,
                            method:
                                YouTubePresentationDataServices.loadPresentationMethodName,
                            output: response.json()
                        });

                        done();
                    })
                    .catch(responseOrVoid => {
                        console.warn({
                            catchResponse: true,
                            service: YouTubePresentationDataServices.name,
                            method:
                                YouTubePresentationDataServices.loadPresentationMethodName,
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
            }
        )();
    });

    it(`should call ${
        YouTubePresentationDataServices.loadVideosMethodName
    }()`, done => {
        inject(
            [YouTubePresentationDataServices],
            (services: YouTubePresentationDataServices) => {
                expect(services).not.toBeNull(
                    'the expected service is not here'
                );

                const id = 'bowie0';

                services
                    .loadVideos(id)
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

                        const service = services.videosDataService;

                        expect(service).toBeDefined(
                            'The expected service is not defined.'
                        );
                        expect(service).not.toBeNull(
                            'The expected service is not here.'
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

                        console.log({
                            serviceName: YouTubePresentationDataServices.name,
                            service,
                            method:
                                YouTubePresentationDataServices.loadVideosMethodName,
                            output: response.json()
                        });

                        done();
                    })
                    .catch(response => {
                        console.log({
                            service: `${
                                YouTubePresentationDataServices.name
                            } [catch response]`,
                            method:
                                YouTubePresentationDataServices.loadVideosMethodName,
                            response: response
                        });

                        done();
                    });
            }
        )();
    });
});
