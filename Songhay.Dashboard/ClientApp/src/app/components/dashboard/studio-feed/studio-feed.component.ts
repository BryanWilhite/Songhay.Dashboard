import { Component, OnInit } from '@angular/core';
import { DashboardDataService } from '../../../services/dashboard-data.service';

@Component({
    selector: 'app-studio-feed',
    templateUrl: './studio-feed.component.html',
    styleUrls: ['./studio-feed.component.scss']
})
export class StudioFeedComponent implements OnInit {
    feedItems: { title: string; link: string }[];
    feedTitle: string;

    constructor(private dashService: DashboardDataService) {}

    ngOnInit() {
        this.dashService.appDataLoaded.subscribe((feed: Map<string, any>) => {
            const rawFeed: any = feed['studio'];
            if (!rawFeed) {
                console.log('the expected raw feed is not here');
                return;
            }

            const channel: any = rawFeed['rss']['channel'];
            if (!channel) {
                console.log('the expected RSS channel is not here');
                return;
            }
            this.feedTitle = channel['title'];

            const channelItems: {}[] = rawFeed['rss']['channel']['item'];
            if (!channelItems) {
                console.log('the expected RSS channel root is not here');
                return;
            }
            if (!channelItems.length) {
                console.log('the expected RSS channel items are not here');
                return;
            }
            this.feedItems = channelItems.map(item => {
                return { title: item['title'], link: item['link'] };
            });
        });
    }
}
