import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { AppDataService } from './songhay-app-data.service';
import { AppScalars } from '../models/songhay-app-scalars';
import { AssemblyInfo } from '../models/songhay-assembly-info';

/**
 * data service of this App
 *
 * @export
 * @class DashboardDataService
 * @extends {AppDataService}
 */
@Injectable()
export class DashboardDataService extends AppDataService {
    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof BlogEntriesService
     */
    static loadFeedsMethodName = 'loadFeeds';

    /**
     * Returns server assembly info.
     *
     * @type {AssemblyInfo}
     * @memberof BlogEntriesService
     */
    assemblyInfo: AssemblyInfo;

    /**
     * map of RSS/Atom feeds
     *
     * @type {Map<string, any>}
     * @memberof DashboardDataService
     */
    feeds: Map<string, any>;

    /**
     * emits event when loadAppData resolves
     *
     * @type {EventEmitter<Map<string, any>>}
     * @memberof DashboardDataService
     */
    @Output()
    appDataLoaded: EventEmitter<Map<string, any>>;

    /**
     * creates an instance of DashboardDataService.
     *
     * @param {Http} client
     * @memberof DashboardDataService
     */
    constructor(client: Http) {
        super(client);

        this.appDataLoaded = new EventEmitter<Map<string, any>>();

        this.initialize();
    }

    /**
     * Promises to load index data.
     *
     * @returns {Promise<HttpResponse>}
     * @memberof BlogEntriesService
     */
    loadAppData(): Promise<Response> {
        this.initialize();

        const rejectionExecutor = (response: Response, reject: any) => {
            this.feeds = response.json()['feeds'] as Map<string, any>;

            if (!this.feeds) {
                reject('feeds map is not truthy.');
                return;
            }

            this.assemblyInfo = response.json()['serverMeta'][
                'assemblyInfo'
            ] as AssemblyInfo;

            if (!this.assemblyInfo) {
                reject('assemblyInfo is not truthy.');

                return;
            }

            this.appDataLoaded.emit(this.feeds);
        };

        const promise = new Promise<Response>(
            super.getExecutor(AppScalars.appDataLocation, rejectionExecutor)
        );

        return promise;
    }

    private initialize(): void {
        this.feeds = null;

        super.initializeLoadState();
    }
}
