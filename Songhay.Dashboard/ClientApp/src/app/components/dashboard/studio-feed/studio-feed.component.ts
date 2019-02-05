import { Component, Input, OnInit } from '@angular/core';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { AppScalars } from '../../../models/songhay-app-scalars';

import { SyndicationFeed } from 'songhay-core/models/syndication-feed';

/**
 * displays syndication feeds
 *
 * @export
 * @class StudioFeedComponent
 * @implements {OnInit}
 */
@Component({
    selector: 'app-studio-feed',
    templateUrl: './studio-feed.component.html',
    styleUrls: ['./studio-feed.component.scss']
})
export class StudioFeedComponent implements OnInit {
    /**
     * the feed to visualize
     *
     * @type {SyndicationFeed}
     * @memberof StudioFeedComponent
     */
    feed: SyndicationFeed;

    /**
     * configured name of the syndication feed
     *
     * @type {string}
     * @memberof StudioFeedComponent
     */
    @Input()
    feedName: string;

    /**
     *Creates an instance of StudioFeedComponent.
     * @param {DashboardDataService} dashService
     * @memberof StudioFeedComponent
     */
    constructor(public dashService: DashboardDataService) {
        this.feed = new SyndicationFeed();
        this.feedName = AppScalars.feedNameStudio;
    }

    /**
     * implementing {OnInit}
     *
     * @memberof StudioFeedComponent
     */
    ngOnInit() {
        this.dashService.appDataLoaded.subscribe(
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
