import { Injectable } from '@angular/core';
import { AppDataService } from './songhay-app-data.service';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { AppScalars } from '../models/songhay-app-scalars';
import { AssemblyInfo } from '../models/songhay-assembly-info';
import { DisplayItemModel } from '../models/songhay-display-item-model';

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
     * @type {Map<string, DisplayItemModel>}
     * @memberof DashboardDataService
     */
    feeds: Map<string, DisplayItemModel>;

    /**
     * creates an instance of DashboardDataService.
     *
     * @param {Http} client
     * @memberof DashboardDataService
     */
    constructor(client: Http) {
        super(client);
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
            const map = response.json()['feeds'] as Map<string, any>;

            if (!map) {
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
