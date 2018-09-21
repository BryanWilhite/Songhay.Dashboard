import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';
import { YouTubeSetIndex } from '../../models/you-tube-set-index';

@Component({
    selector: 'app-you-tube-thumbs-navigation',
    templateUrl: './you-tube-thumbs-navigation.component.html',
    styleUrls: ['./you-tube-thumbs-navigation.component.scss']
})
export class YouTubeThumbsNavigationComponent implements OnInit {

    channelsIndexName: string;
    channelTitle: string;
    dataForYouTubeSetIndex: YouTubeSetIndex;
    id: string;

    constructor(
        public youTubeDataService: YouTubeDataService,
        private route: ActivatedRoute,
        private router: Router
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
        });

        this.youTubeDataService.loadChannelsIndex(this.channelsIndexName);
        this.youTubeDataService.channelsIndexLoaded.subscribe(json => {
            this.dataForYouTubeSetIndex = json as YouTubeSetIndex;
            this.setChannelTitle();
        });
    }

    doMenuItemCommand(document: { ClientId: string }) {
        if (!document) {
            return;
        }

        this.router.navigate([`/video/youtube/uploads/${document.ClientId}`]);
    }

    private setChannelTitle(): void {
        const document = this.dataForYouTubeSetIndex.documents.find(i => {
            return i.clientId === this.id;
        });
        this.channelTitle = document.title;
    }
}
