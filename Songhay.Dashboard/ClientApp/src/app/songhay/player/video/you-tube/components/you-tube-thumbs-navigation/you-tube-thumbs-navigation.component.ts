import * as _ from 'lodash';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';

@Component({
    selector: 'app-you-tube-thumbs-navigation',
    templateUrl: './you-tube-thumbs-navigation.component.html',
    styleUrls: ['./you-tube-thumbs-navigation.component.scss']
})
export class YouTubeThumbsNavigationComponent implements OnInit {
    channelsIndexName: string;
    channelTitle: string;
    dataForYouTubeSetIndex: {
        Documents: [{ ClientId: string; Title: string }];
    };
    id: string;
    isSetLoaded: boolean;
    isSetLoading: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private dataServiceForYouTube: YouTubeDataService
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
        });

        this.loadChannelSetIndex();
    }

    doMenuItemCommand(document: { ClientId: string }) {
        if (!document) {
            return;
        }

        this.router.navigate(['/video/youtube/uploads/' + document.ClientId]);
    }

    isNotExpectedResponse(response: Response) {
        // TODO: centralize with decorator.
        return !response || response.status === 404 || response.status === -1;
    }

    loadChannelSetIndex() {
        console.log('calling scope.directiveVM.loadChannelSetIndex...');
        if (!this.channelsIndexName) {
            console.log(
                'The expected Channels Index Name is not here.',
                this.channelsIndexName
            );
            return;
        }
        this.isSetLoading = true;

        this.dataServiceForYouTube
            .loadChannelsIndex(this.channelsIndexName)
            .then(function(dataOrErrorResponse) {
                console.log(
                    'dataServiceForYouTube.loadChannelsIndex promised:',
                    dataOrErrorResponse
                );
                if (this.isNotExpectedResponse(dataOrErrorResponse)) {
                    console.log('The expected data is not here.');
                    return;
                }

                this.isSetLoaded = true;
                this.isSetLoading = false;
                this.dataForYouTubeSetIndex = dataOrErrorResponse;
                this.setChannelTitle();
            });
    }

    setChannelTitle() {
        const document = _(this.dataForYouTubeSetIndex.Documents).find(function(
            i
        ) {
            return i.ClientId === this.id;
        });
        this.channelTitle = document.Title;
    }
}
