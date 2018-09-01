import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit
} from '@angular/core';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { SyndicationFeed } from '../../../models/songhay-syndication-feed';

/**
 * displays syndication feeds
 *
 * @export
 * @class StudioFeedComponent
 * @implements {OnInit}
 */
@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
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
    constructor(private dashService: DashboardDataService) {
        this.feed = new SyndicationFeed();
    }

    /**
     * implementing {OnInit}
     *
     * @memberof StudioFeedComponent
     */
    ngOnInit() {
        this.dashService.appDataLoaded.subscribe(
            (feed: Map<string, SyndicationFeed>) => {
                this.feed = this.dashService.feeds.get(this.feedName);
            }
        );
    }
}
