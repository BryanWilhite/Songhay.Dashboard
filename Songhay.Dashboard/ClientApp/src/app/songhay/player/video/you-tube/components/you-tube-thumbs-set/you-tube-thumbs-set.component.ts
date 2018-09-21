import { Location } from '@angular/common';
import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';
import { YouTubeItem } from '../../models/you-tube-item';

@Component({
    selector: 'app-you-tube-thumbs-set',
    templateUrl: './you-tube-thumbs-set.component.html',
    styleUrls: ['./you-tube-thumbs-set.component.scss']
})
export class YouTubeThumbsSetComponent implements OnInit {
    @Input()
    thumbsSetSuffix: string;

    youTubeItems: Map<string, YouTubeItem[]>;

    private id: string;
    private suffix: string;

    constructor(
        public youTubeDataService: YouTubeDataService,
        private location: Location,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
            this.suffix = params['suffix'] as string;
        });

        this.youTubeDataService
            .loadChannelSet(this.suffix, this.id)
            .catch(() => {
                console.warn({
                    component: YouTubeThumbsSetComponent.name,
                    id: this.id,
                    message: 'The expected data is not here.'
                });

                if (this.id) {
                    this.location.replaceState('/not-found');
                }
            });

        this.youTubeDataService.channelSetLoaded.subscribe(json => {
            const set = Array.from(json['set']).filter(i => {
                const test = i && i['items'];
                if (!test) {
                    console.warn({
                        component: YouTubeThumbsSetComponent.name,
                        message: 'channelSetLoaded: item filtered out',
                        itemFilteredOut: i
                    });
                }
                return test;
            });
            this.youTubeItems = new Map(set.map(o => {
                const items = o['items'] as YouTubeItem[];
                const key = items[0].snippet.channelTitle;
                return [key, items] as [string, YouTubeItem[]];
            }));
        });
    }
}
