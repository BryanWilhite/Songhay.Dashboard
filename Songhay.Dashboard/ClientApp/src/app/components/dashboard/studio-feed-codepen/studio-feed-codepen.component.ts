import { Component, OnInit } from '@angular/core';
import { DashboardDataService } from '../../../services/dashboard-data.service';

@Component({
    selector: 'app-studio-feed-codepen',
    templateUrl: './studio-feed-codepen.component.html',
    styleUrls: ['./studio-feed-codepen.component.scss']
})
export class StudioFeedCodepenComponent implements OnInit {
    feedImage: string;
    feedItems: { title: string; link: string }[];
    feedTitle: string;

    constructor(private dashService: DashboardDataService) {}

    ngOnInit() {
        this.dashService.appDataLoaded.subscribe((feed: Map<string, any>) => {
            const rawFeed: any = feed['codepen'];
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
            this.feedImage = `${this.feedItems[0].link}/image/large.png`;
        });
    }
}
