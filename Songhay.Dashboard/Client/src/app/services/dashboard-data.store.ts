import { Injectable } from '@angular/core';

import { AppDataStore, AppDataStoreOptions } from '@songhay/core';

import { SyndicationFeed } from 'songhay/core/models/syndication-feed';

import { AppScalars } from '../models/songhay-app-scalars';
import { HttpClient } from '@angular/common/http';
import { MapObjectUtility } from 'songhay/core/utilities/map-object.utility';

@Injectable()
export class DashboardDataStore extends AppDataStore<Map<string, SyndicationFeed>, any> {

    static getFeed(key: string, rawFeed: any): SyndicationFeed {
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
                        title: item['title']['#text'],
                        link: item['link']['@href']
                    };
                });
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
}
