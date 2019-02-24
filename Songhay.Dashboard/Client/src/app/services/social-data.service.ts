import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import { AppDataService } from '@songhay/core';
import { AppScalars } from '../models/songhay-app-scalars';
import { TwitterItem } from '../models/twitter-item';

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
    twitterItemsLoaded: EventEmitter<TwitterItem[]>;

    /**
     *Creates an instance of SocialDataService.
     * @param {Http} client
     * @memberof SocialDataService
     */
    constructor(client: Http) {
        super(client);

        this.twitterItemsLoaded = new EventEmitter();
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

        return this.loadJson<{}>(uri, json => {
            const items = json as TwitterItem[];
            this.twitterItemsLoaded.emit(items);
        });
    }

    private initialize(): void {
        super.initializeLoadState();
    }
}
