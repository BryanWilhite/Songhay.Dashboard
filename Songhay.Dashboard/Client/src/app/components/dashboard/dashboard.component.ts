import { Component, OnInit } from '@angular/core';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';
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
        public dashStore: DashboardDataStore,
        public youTubeDataService: YouTubeDataService
    ) { }

    ngOnInit(): void {
        this.dashStore.loadAppData();
        this.youTubeDataService.loadChannel('youtube-index-songhay-top-ten');

        this.youTubeDataService.channelLoaded.subscribe(json => {
            this.youTubeItems = YouTubeDataService.getItems(json);
        });
    }
}
