import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { CssUtility } from '../../../../../core/services/songhay-css.utility';
import { YouTubeDataService } from '../../services/you-tube-data.service';

@Component({
    selector: 'app-you-tube-presentation',
    templateUrl: './you-tube-presentation.component.html',
    styleUrls: ['./you-tube-presentation.component.scss']
})
export class YouTubePresentationComponent implements OnInit {
    areVideosLoaded: boolean;

    areVideosLoading: boolean;

    dataForYouTubeChannel: {};

    dataForYouTubePresentation: {
        presentation: {
            LayoutMetadata: { playlist: {}; prose: string; title: string };
        };
        videos: [];
    };

    id: string;

    isPresentationLoaded: boolean;

    isPresentationLoading: boolean;

    style: {
        playlist: {};
        prose: {};
        title: {};
    };

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private dataServiceForYouTube: YouTubeDataService
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
        });

        this.loadPresentation();
    }
    getBodyStyle() {
        const data = this.dataForYouTubePresentation.presentation
            .LayoutMetadata;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            )
        };
    }
    getPlaylistStyle() {
        const data = this.dataForYouTubePresentation.presentation.LayoutMetadata
            .playlist;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }
    getProseStyle() {
        const data = this.dataForYouTubePresentation.presentation.LayoutMetadata
            .prose;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }
    getTitleStyle() {
        const data = this.dataForYouTubePresentation.presentation.LayoutMetadata
            .title;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color']),
            display: CssUtility.getPixelValue(data['@display'])
        };
    }
    isNotExpectedResponse(response) {
        // TODO: centralize with decorator.
        return !response || response.status === 404 || response.status === -1;
    }
    loadPresentation() {
        console.log('calling scope.youTubeVM.loadPresentation...');
        this.areVideosLoading = true;
        this.dataServiceForYouTube
            .loadVideos(this.id)
            .then(function(dataOrErrorResponse) {
                console.log(
                    'dataServiceForYouTube.loadVideos promised:',
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

                this.dataForYouTubePresentation.videos = dataOrErrorResponse;
                this.areVideosLoaded = true;
                this.areVideosLoading = false;
            });

        this.isPresentationLoading = true;
        this.dataServiceForYouTube
            .loadPresentation(this.id)
            .then((dataOrErrorResponse: any) => {
                console.log(
                    'dataServiceForYouTube.loadPresentation promised:',
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

                this.dataForYouTubePresentation.presentation =
                    dataOrErrorResponse.Presentation;
                this.isPresentationLoaded = true;
                this.isPresentationLoading = false;

                // if (scope.clientVM) {
                //     scope.clientVM.style.body = this.getBodyStyle();
                // }
                this.style.playlist = this.getPlaylistStyle();
                this.style.prose = this.getProseStyle();
                this.style.title = this.getTitleStyle();
            });
    }
}
