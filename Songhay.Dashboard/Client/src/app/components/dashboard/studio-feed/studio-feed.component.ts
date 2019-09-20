import { Component, Input, OnInit } from '@angular/core';

import { AppScalars } from '../../../models/songhay-app-scalars';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

import { SyndicationFeed } from 'songhay/core/models/syndication-feed';

/**
 * displays syndication feeds
 */
@Component({
    selector: 'app-studio-feed',
    templateUrl: './studio-feed.component.html',
    styleUrls: ['./studio-feed.component.scss']
})
export class StudioFeedComponent implements OnInit {
    /**
     * the feed to visualize
     */
    feed: SyndicationFeed;

    /**
     * configured name of the syndication feed
     */
    @Input()
    feedName: string;

    /**
     * Creates an instance of StudioFeedComponent.
     */
    constructor(public dashStore: DashboardDataStore) {
        this.feed = new SyndicationFeed();
        this.feedName = AppScalars.feedNameStudio;
    }

    /**
     * implementing {OnInit}
     */
    ngOnInit() {
        this.dashStore.serviceData.subscribe(
            (feeds: Map<string, SyndicationFeed>) => {
                const feed = feeds.get(this.feedName);

                if (feed.feedImage) {
                    this.feed.feedImage = feed.feedImage;
                }
                this.feed.feedItems = feed.feedItems;
                this.feed.feedTitle = feed.feedTitle;
            }
        );
    }
}
