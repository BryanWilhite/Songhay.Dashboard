import { Location } from '@angular/common';
import * as _ from 'lodash';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { YouTubeDataService } from '../../services/you-tube-data.service';

@Component({
    selector: 'app-you-tube-thumbs-set',
    templateUrl: './you-tube-thumbs-set.component.html',
    styleUrls: ['./you-tube-thumbs-set.component.scss']
})
export class YouTubeThumbsSetComponent implements OnInit {
    dataForYouTubeSet: {};
    id: string;
    isSetLoaded: boolean;
    isSetLoading: boolean;
    thumbsSetSuffix: string;

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private youTubeDataService: YouTubeDataService
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
        });

        this.loadChannelSet();
    }

    isNotExpectedResponse(response) {
        // TODO: centralize with decorator.
        return !response || response.status === 404 || response.status === -1;
    }

    loadChannelSet() {
        console.log('calling scope.directiveVM.loadChannelSet...');
        this.isSetLoading = true;
        this.youTubeDataService
            .loadChannelSet(this.id)
            .then((dataOrErrorResponse: { set: [{ items: [] }] }) => {
                console.log(
                    'dataServiceForYouTube.loadChannelSet promised:',
                    dataOrErrorResponse
                );
                if (this.isNotExpectedResponse(dataOrErrorResponse)) {
                    console.log(
                        'The expected data is not here.',
                        '[id:',
                        this.id,
                        ']'
                    );
                    if (this.id) {
                        this.location.replaceState('/not-found');
                    }
                    return;
                }

                this.isSetLoaded = true;
                this.isSetLoading = false;
                this.dataForYouTubeSet = _(dataOrErrorResponse.set).filter(
                    (i) => {
                        const test = i && i.items;
                        if (!test) {
                            console.log('filtered out: ', i);
                        }
                        return test;
                    }
                );
            });
    }
}
