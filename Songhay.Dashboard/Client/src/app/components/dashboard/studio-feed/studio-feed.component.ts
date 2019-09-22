import { Subscription } from 'rxjs';

import { Component, Input, OnInit, OnDestroy } from '@angular/core';

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
export class StudioFeedComponent implements OnInit, OnDestroy {
    /**
     * the feed to visualize
     */
    feed: SyndicationFeed;

    /**
     * configured name of the syndication feed
     */
    @Input()
    feedName: string;

    private subscriptions: Subscription[] = [];

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
        const sub = this.dashStore.serviceData.subscribe(
            (feeds: Map<string, SyndicationFeed>) => {
                const feed = feeds.get(this.feedName);

                if (!feed) { return; }

                if (feed.feedImage) {
                    this.feed.feedImage = feed.feedImage;
                }
                this.feed.feedItems = feed.feedItems;
                this.feed.feedTitle = feed.feedTitle;
            }
        );

        this.subscriptions.push(sub);
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }

}
