import { Subscription } from 'rxjs';

import { Component, OnInit, OnDestroy } from '@angular/core';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';
import { YouTubeChannelDataStore, YouTubeItem, YouTubeRouteUtility } from '@songhay/player-video-you-tube';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {

    titleRouterLink = YouTubeRouteUtility.getUploadsRoute('songhay', 'news');

    youTubeItems: YouTubeItem[];

    private subscriptions: Subscription[] = [];

    constructor(
        public dashStore: DashboardDataStore,
        public youTubeChannelDataStore: YouTubeChannelDataStore
    ) { }

    ngOnInit(): void {
        const id = 'youtube-index-songhay-top-ten';
        const uri = YouTubeChannelDataStore.getUri('get', id);

        this.dashStore.loadAppData();

        const sub = this.youTubeChannelDataStore.serviceData.subscribe(data => {
            this.youTubeItems = data;
        });

        this.subscriptions.push(sub);

        this.youTubeChannelDataStore.load(uri);
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }
}
