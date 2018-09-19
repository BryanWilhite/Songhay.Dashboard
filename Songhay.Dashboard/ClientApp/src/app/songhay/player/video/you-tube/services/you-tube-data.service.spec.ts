import { TestBed, inject } from '@angular/core/testing';
import {
    BaseRequestOptions,
    Http,
    HttpModule,
    Response,
    XHRBackend
} from '@angular/http';

import { YouTubeDataService } from './you-tube-data.service';
import { YouTubeScalars } from '../models/you-tube-scalars';

describe(YouTubeDataService.name, () => {
    YouTubeScalars.setupForJasmine();

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
});
