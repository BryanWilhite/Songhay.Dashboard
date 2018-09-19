import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';

import { YouTubePresentationDataServices } from './you-tube-presentation-data.services';
import { YouTubeScalars } from '../models/you-tube-scalars';

describe(YouTubePresentationDataServices.name, () => {
    YouTubeScalars.setupForJasmine();

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
});
