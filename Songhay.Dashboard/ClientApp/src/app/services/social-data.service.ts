import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AppDataService } from '../songhay/core/services/songhay-app-data.service';
import { AppScalars } from '../models/songhay-app-scalars';

/**
 * Social data service
 *
 * @export
 * @class SocialDataService
 * @extends {AppDataService}
 */
@Injectable({
    providedIn: 'root'
})
export class SocialDataService extends AppDataService {
    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof SocialDataService
     */
    static loadTwitterItemsMethodName = 'loadTwitterItems';

    /**
     * emits event when @member loadTwitterItems resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof SocialDataService
     */
    @Output()
    twitterItemsLoaded: EventEmitter<{}>;

    /**
     *Creates an instance of SocialDataService.
     * @param {Http} client
     * @memberof SocialDataService
     */
    constructor(client: Http) {
        super(client);

        this.twitterItemsLoaded = new EventEmitter();
        this.initialize();
    }

    /**
     * loads Twitter status items
     *
     * @returns {Promise<Response>}
     * @memberof SocialDataService
     */
    loadTwitterItems(): Promise<Response> {
        this.initialize();

        const uri = `${AppScalars.rxTwitterApiRootUri}statuses`;

        return this.loadJson<{}>(uri, json =>
            this.twitterItemsLoaded.emit(json)
        );
    }

    private initialize(): void {
        super.initializeLoadState();
    }
}
