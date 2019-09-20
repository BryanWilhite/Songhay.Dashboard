import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { AssemblyInfo } from 'songhay/core/models/assembly-info';
import { SyndicationFeed } from 'songhay/core/models/syndication-feed';
import { SyndicationFeedItem } from 'songhay/core/models/syndication-feed-item';
import { MapObjectUtility } from 'songhay/core/utilities/map-object.utility';

import { AppDataStore, AppDataStoreOptions } from '@songhay/core';

import { AppScalars } from '../models/songhay-app-scalars';

@Injectable()
export class DashboardDataStore extends AppDataStore<Map<string, SyndicationFeed>, any> {
    /**
     * Returns server assembly info.
     */
    assemblyInfo: AssemblyInfo;

    static getFeed(key: string, rawFeed: any): SyndicationFeed {
        const feed = new SyndicationFeed();

        const getFlickrFeedImage = (item: {}) => {
            const o = item as { enclosure: {} };
            if (!o) { return null; }
            return o.enclosure['@url'] as string;
        };

        const mapCustomItem = (item: { title: {}, link: {} }) => {
            return {
                title: item.title['#text'],
                link: item.link['@href']
            } as SyndicationFeedItem;
        };

        let channelItems: {}[];

        switch (key) {
            case AppScalars.feedNameCodePen:
            case AppScalars.feedNameFlickr:
            case AppScalars.feedNameStudio:
                feed.feedTitle = SyndicationFeed.getRssChannelTitle(rawFeed);
                channelItems = SyndicationFeed.getRssChannelItems(rawFeed);
                feed.feedItems = channelItems.map(item => item as SyndicationFeedItem);
                break;

            case AppScalars.feedNameGitHub:
                feed.feedTitle = SyndicationFeed.getAtomChannelTitle(rawFeed);
                channelItems = SyndicationFeed.getAtomChannelItems(rawFeed);
                break;

            case AppScalars.feedNameStackOverflow:
                feed.feedTitle = SyndicationFeed
                    .getAtomChannelTitle(rawFeed)
                    .replace('User rasx - ', '');
                channelItems = SyndicationFeed.getAtomChannelItems(rawFeed);
                break;
        }

        switch (key) {
            case AppScalars.feedNameCodePen:
                feed.feedImage = `${feed.feedItems[0].link}/image/large.png`;
                break;
            case AppScalars.feedNameFlickr:
                feed.feedTitle = feed.feedTitle.replace('Content from ', '');
                feed.feedImage = getFlickrFeedImage(channelItems[0]);
                break;
            case AppScalars.feedNameGitHub:
                feed.feedItems = channelItems
                    .map(mapCustomItem)
                    .map(item => ({
                        title: item.title.replace(
                            'BryanWilhite ',
                            ''
                        ),
                        link: item.link
                    }));
                break;
            case AppScalars.feedNameStackOverflow:
                feed.feedItems = channelItems.map(mapCustomItem);
                break;
        }

        return feed;
    }

    constructor(client: HttpClient) {
        super(client);

        const options: AppDataStoreOptions<Map<string, SyndicationFeed>, any> = {
            domainConverter: (method, data) => {
                switch (method) {
                    default:
                    case 'get':
                        const sm = data as { serverMeta: { assemblyInfo: AssemblyInfo } };
                        this.assemblyInfo = sm.serverMeta.assemblyInfo;

                        return MapObjectUtility.getMap<SyndicationFeed>(
                            data,
                            (propertyName, propertyValue) =>
                                DashboardDataStore.getFeed(propertyName, propertyValue)
                        );
                }
            }
        };

        this.options = options;
    }

    loadAppData(): void {
        this.load(AppScalars.appDataLocation);
    }
}
