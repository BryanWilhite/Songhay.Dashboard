import { Component, OnInit } from '@angular/core';
import { DashboardDataService } from '../../../services/dashboard-data.service';

@Component({
    selector: 'app-studio-feed',
    templateUrl: './studio-feed.component.html',
    styleUrls: ['./studio-feed.component.scss']
})
export class StudioFeedComponent implements OnInit {
    constructor(private dashService: DashboardDataService) {}

    ngOnInit() {
        this.dashService.appDataLoaded.subscribe((feed: Map<string, any>) => {
            console.log('yup:appDataLoaded');
        });
    }
}
