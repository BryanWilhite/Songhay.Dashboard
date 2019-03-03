import { Component, OnInit } from '@angular/core';

import { DashboardDataService } from '../../services/dashboard-data.service';
import { YouTubeItem, YouTubeDataService, YouTubeRouteUtility } from '@songhay/player-video-you-tube';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

    titleRouterLink = YouTubeRouteUtility.getUploadsRoute('songhay', 'news');

    youTubeItems: YouTubeItem[];

    constructor(
        public dashService: DashboardDataService,
        public youTubeDataService: YouTubeDataService
    ) { }

    ngOnInit(): void {
        this.dashService.loadAppData();
        this.youTubeDataService.loadChannel('youtube-index-songhay-top-ten');

        this.youTubeDataService.channelLoaded.subscribe(json => {
            this.youTubeItems = YouTubeDataService.getItems(json);
        });
    }
}
