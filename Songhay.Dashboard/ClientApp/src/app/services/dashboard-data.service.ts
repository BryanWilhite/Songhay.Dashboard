import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { AppDataService } from './songhay-app-data.service';
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
     * @type {Map<string, DisplayItemModel[]>}
     * @memberof DashboardDataService
     */
    feeds: Map<string, DisplayItemModel[]>;

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
            const rawMap = response.json()['feeds'] as Map<string, any>;

            if (!rawMap) {
                reject('feeds map is not truthy.');
                return;
            }

            Array.from(rawMap.keys()).map(key => {
                const rawFeed: any = rawMap.get(key);
                let models: DisplayItemModel[];
                switch (key) {
                    case 'github':
                        models = this.getAtomDisplayItemModels(rawFeed);
                        break;
                    default:
                        models = this.getRssDisplayItemModels(rawFeed);
                }
                this.feeds.set(key, models);
            });

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

    private getAtomDisplayItemModels(rawFeed: any): DisplayItemModel[] {
        throw new Error('Method not implemented.');
    }

    private getRssDisplayItemModels(rawFeed: any): DisplayItemModel[] {
        throw new Error('Method not implemented.');
    }

    private initialize(): void {
        this.feeds = new Map<string, DisplayItemModel[]>();

        super.initializeLoadState();
    }
}
