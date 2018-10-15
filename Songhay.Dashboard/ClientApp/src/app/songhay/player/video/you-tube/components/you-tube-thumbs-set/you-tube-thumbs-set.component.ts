import { Location } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';
import { YouTubeItem } from '../../models/you-tube-item';

@Component({
    selector: 'rx-you-tube-thumbs-set',
    templateUrl: './you-tube-thumbs-set.component.html',
    styleUrls: ['./you-tube-thumbs-set.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default
})
export class YouTubeThumbsSetComponent implements OnInit {
    @Input()
    thumbsSetSuffix: string;

    youTubeItemsKeys: string[];
    youTubeItemsMap: Map<string, YouTubeItem[]>;

    constructor(
        public youTubeDataService: YouTubeDataService,
        private location: Location,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            const id = params['id'] as string;
            const suffix = params['suffix'] as string;

            this.youTubeDataService.loadChannelSet(suffix, id).catch(() => {
                console.warn({
                    component: YouTubeThumbsSetComponent.name,
                    id,
                    message: 'The expected data is not here.'
                });

                if (id) {
                    this.location.replaceState('/not-found');
                }
            });
        });

        this.youTubeDataService.channelSetLoaded.subscribe(json => {
            this.youTubeItemsMap = YouTubeDataService.getItemsMap(json);
            this.youTubeItemsKeys = Array.from(this.youTubeItemsMap.keys()).sort();
        });
    }
}
