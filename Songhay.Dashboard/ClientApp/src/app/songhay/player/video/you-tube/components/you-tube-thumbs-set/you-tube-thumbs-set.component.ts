import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';

@Component({
    selector: 'app-you-tube-thumbs-set',
    templateUrl: './you-tube-thumbs-set.component.html',
    styleUrls: ['./you-tube-thumbs-set.component.scss']
})
export class YouTubeThumbsSetComponent implements OnInit {
    dataForYouTubeSet: {};
    thumbsSetSuffix: string;

    private id: string;
    private suffix: string;

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private youTubeDataService: YouTubeDataService
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
            this.suffix = params['suffix'] as string;
        });

        this.youTubeDataService.loadChannelSet(this.suffix , this.id).catch(() => {
            console.log('The expected data is not here.', '[id:', this.id, ']');
            if (this.id) {
                this.location.replaceState('/not-found');
            }
        });

        this.youTubeDataService.channelSetLoaded.subscribe(json => {
            const set = Array.from(json['set']).filter(i => {
                const test = i && i['items'];
                if (!test) {
                    console.log('filtered out: ', i);
                }
                return test;
            });
        });
    }
}
