import { TestBed } from '@angular/core/testing';
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
});
