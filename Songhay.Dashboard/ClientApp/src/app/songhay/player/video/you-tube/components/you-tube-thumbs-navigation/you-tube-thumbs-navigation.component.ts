import {
    ChangeDetectionStrategy,
    Component,
    OnInit,
    Input
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';

@Component({
    selector: 'rx-you-tube-thumbs-navigation',
    templateUrl: './you-tube-thumbs-navigation.component.html',
    styleUrls: ['./you-tube-thumbs-navigation.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default
})
export class YouTubeThumbsNavigationComponent implements OnInit {
    @Input()
    channelsIndexName: string;

    channels: { ClientId: string; Title: string }[];
    channelsName: string;
    channelTitle: string;

    private channelSetId: string;

    constructor(
        public youTubeDataService: YouTubeDataService,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.channelSetId = params['id'] as string;
            this.setChannelTitle();
        });

        this.youTubeDataService.loadChannelsIndex(this.channelsIndexName);

        this.youTubeDataService.channelsIndexLoaded.subscribe(json => {
            this.channelsName = json['SegmentName'];
            this.setChannels(json);
            this.setChannelTitle();
        });
    }

    private setChannels(json: {}): void {
        const docs = json['Documents'] as {}[];
        if (!docs) {
            console.warn({
                name: YouTubeThumbsNavigationComponent.name,
                message: 'the expected channel documents are not here'
            });
            return;
        }
        this.channels = docs.map(o => {
            return { ClientId: o['ClientId'], Title: o['Title'] };
        });
    }

    private setChannelTitle(): void {
        if (!this.channels) {
            return;
        }

        const document = this.channels.find(i => {
            return i.ClientId === this.channelSetId;
        });
        this.channelTitle = document.Title;
    }
}
