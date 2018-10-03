import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';

import { YouTubeDataService } from './you-tube-data.service';

describe(YouTubeDataService.name, () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpModule],
            providers: [
                BaseRequestOptions,
                YouTubeDataService,
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
        [YouTubeDataService],
        (service: YouTubeDataService) => {
            expect(service).toBeTruthy();
        }
    ));

    it(`should call ${YouTubeDataService.loadChannelMethodName}()`, done => {
        inject([YouTubeDataService], (service: YouTubeDataService) => {
            expect(service).not.toBeNull('the expected service is not here');

            const channelId = 'youtube-index-songhay-top-ten';

            service
                .loadChannel(channelId)
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

                    console.log({
                        service: YouTubeDataService.name,
                        method: YouTubeDataService.loadChannelMethodName,
                        output: response.json()
                    });

                    done();
                })
                .catch(responseOrVoid => {
                    console.warn({
                        catchResponse: true,
                        service: YouTubeDataService.name,
                        method: YouTubeDataService.loadChannelMethodName,
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

    it(`should call ${YouTubeDataService.loadChannelSetMethodName}()`, done => {
        inject([YouTubeDataService], (service: YouTubeDataService) => {
            expect(service).not.toBeNull('the expected service is not here');

            const suffix = 'songhay';
            const id = 'news';

            service
                .loadChannelSet(suffix, id)
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

                    console.log({
                        service: YouTubeDataService.name,
                        method: YouTubeDataService.loadChannelSetMethodName,
                        output: response.json()
                    });

                    done();
                })
                .catch(response => {
                    console.log({
                        service: `${YouTubeDataService.name} [catch response]`,
                        method: YouTubeDataService.loadChannelSetMethodName,
                        response: response
                    });

                    done();
                });
        })();
    });

    it(`should call ${
        YouTubeDataService.loadChannelsIndexMethodName
    }()`, done => {
        inject([YouTubeDataService], (service: YouTubeDataService) => {
            expect(service).not.toBeNull('the expected service is not here');

            const suffix = 'songhay';

            service
                .loadChannelsIndex(suffix)
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

                    console.log({
                        service: YouTubeDataService.name,
                        method: YouTubeDataService.loadChannelsIndexMethodName,
                        output: response.json()
                    });

                    done();
                })
                .catch(response => {
                    console.log({
                        service: `${YouTubeDataService.name} [catch response]`,
                        method: YouTubeDataService.loadChannelsIndexMethodName,
                        response: response
                    });

                    done();
                });
        })();
    });
});
