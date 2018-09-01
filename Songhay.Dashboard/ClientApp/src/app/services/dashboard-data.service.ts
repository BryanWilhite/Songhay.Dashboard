import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { AppDataService } from './songhay-app-data.service';
import { AppScalars } from '../models/songhay-app-scalars';
import { AssemblyInfo } from '../models/songhay-assembly-info';
import { SyndicationFeed } from '../models/songhay-syndication-feed';

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
     * @type {Map<string, SyndicationFeed>}
     * @memberof DashboardDataService
     */
    feeds: Map<string, SyndicationFeed>;

    /**
     * emits event when loadAppData resolves
     *
     * @type {EventEmitter<Map<string, SyndicationFeed>>}
     * @memberof DashboardDataService
     */
    @Output()
    appDataLoaded: EventEmitter<Map<string, SyndicationFeed>>;

    /**
     * creates an instance of DashboardDataService.
     *
     * @param {Http} client
     * @memberof DashboardDataService
     */
    constructor(client: Http) {
        super(client);

        this.appDataLoaded = new EventEmitter<Map<string, SyndicationFeed>>();

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
            const rawFeeds = response.json()['feeds'] as {};

            if (!rawFeeds) {
                reject('raw feeds map is not truthy.');
                return;
            }

            this.feeds = this.getFeeds(rawFeeds);

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

    private getFeed(key: string, rawFeed: any): SyndicationFeed {
        const feed = new SyndicationFeed();
        let channelItems: {}[];

        switch (key) {
            case AppScalars.feedNameCodePen:
            case AppScalars.feedNameFlickr:
            case AppScalars.feedNameStudio:
                feed.feedTitle = SyndicationFeed.getRssChannelTitle(rawFeed);
                channelItems = SyndicationFeed.getRssChannelItems(rawFeed);
                feed.feedItems = channelItems.map(item => {
                    return { title: item['title'], link: item['link'] };
                });
                break;

            case AppScalars.feedNameGitHub:
            case AppScalars.feedNameStackOverflow:
                feed.feedTitle = SyndicationFeed.getAtomChannelTitle(rawFeed);
                channelItems = SyndicationFeed.getAtomChannelItems(rawFeed);
                break;
        }

        switch (key) {
            case AppScalars.feedNameCodePen:
                feed.feedImage = `${feed.feedItems[0].link}/image/large.png`;
                break;
            case AppScalars.feedNameFlickr:
                feed.feedTitle = feed.feedTitle.replace('Content from ', '');
                feed.feedImage = channelItems[0]['enclosure']['@url'];
                break;
            case AppScalars.feedNameGitHub:
                feed.feedItems = channelItems.map(item => {
                    return {
                        title: (item['title']['#text'] as string).replace(
                            'BryanWilhite ',
                            ''
                        ),
                        link: item['link']['@href']
                    };
                });
                break;
            case AppScalars.feedNameStackOverflow:
                feed.feedItems = channelItems.map(item => {
                    return {
                        title: item['title'],
                        link: item['link']['@href']
                    };
                });
                break;
        }

        return feed;
    }

    private getFeeds(rawFeeds: {}): Map<string, SyndicationFeed> {
        const iterable = Object.keys(rawFeeds).map(key => {
            const rawFeed = rawFeeds[key];
            const feed = this.getFeed(key, rawFeed);
            return [key, feed] as [string, SyndicationFeed];
        });
        return new Map<string, SyndicationFeed>(iterable);
    }

    private initialize(): void {
        this.feeds = null;

        super.initializeLoadState();
    }
}
