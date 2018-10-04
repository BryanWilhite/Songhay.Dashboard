import { Component, OnInit } from '@angular/core';

import { SocialDataService } from '../../../services/social-data.service';

@Component({
    selector: 'app-tweeted-links-builder',
    templateUrl: './tweeted-links-builder.component.html',
    styleUrls: ['./tweeted-links-builder.component.scss']
})
export class TweetedLinksBuilderComponent implements OnInit {
    constructor(public socialDataService: SocialDataService) {}

    ngOnInit() {
        this.socialDataService.twitterItemsLoaded.subscribe(json => {});
    }

    getStatuses(): void {
        // this.socialDataService.loadTwitterItems();
    }
}
