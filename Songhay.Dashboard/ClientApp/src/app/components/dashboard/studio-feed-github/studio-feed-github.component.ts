import { Component, OnInit } from '@angular/core';
import { DashboardDataService } from '../../../services/dashboard-data.service';

@Component({
    selector: 'app-studio-feed-github',
    templateUrl: './studio-feed-github.component.html',
    styleUrls: ['./studio-feed-github.component.scss']
})
export class StudioFeedGithubComponent implements OnInit {
    feedItems: { title: string; link: string }[];
    feedTitle: string;

    constructor(private dashService: DashboardDataService) {}

    ngOnInit() {
        this.dashService.appDataLoaded.subscribe((feed: Map<string, any>) => {
            const rawFeed: any = feed['github'];
            if (!rawFeed) {
                console.log('the expected raw feed is not here');
                return;
            }

            const atomFeed: any = rawFeed['feed'];
            if (!atomFeed) {
                console.log('the expected feed is not here');
                return;
            }
            this.feedTitle = atomFeed['title'];

            const channelItems: {}[] = rawFeed['feed']['entry'];
            if (!channelItems) {
                console.log('the expected feed root is not here');
                return;
            }
            if (!channelItems.length) {
                console.log('the expected feed items are not here');
                return;
            }
            this.feedItems = channelItems.map(item => {
                return {
                    title: (item['title']['#text'] as string).replace(
                        'BryanWilhite ',
                        ''
                    ),
                    link: item['link']['@href']
                };
            });
        });
    }
}
