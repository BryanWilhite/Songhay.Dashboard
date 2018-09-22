import { Component, OnInit } from '@angular/core';

import { DashboardDataService } from '../../services/dashboard-data.service';
import { YouTubeDataService } from '../../songhay/player/video/you-tube/services/you-tube-data.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    constructor(
        public dashService: DashboardDataService,
        public youTubeDataService: YouTubeDataService
    ) {}

    ngOnInit(): void {
        this.dashService.loadAppData();
        this.youTubeDataService.loadChannel('youtube-index-songhay-top-ten');

        this.youTubeDataService.channelLoaded.subscribe(json => {
            console.log({json});
        });
    }
}
