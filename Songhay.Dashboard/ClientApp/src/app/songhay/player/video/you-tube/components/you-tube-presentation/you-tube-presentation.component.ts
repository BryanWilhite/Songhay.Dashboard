import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { CssUtility } from '../../../../../core/services/songhay-css.utility';

import { YouTubePresentation } from '../../models/you-tube-presentation';
import { YouTubePresentationStyles } from '../../models/you-tube-presentation-style';

import { YouTubePresentationDataServices } from '../../services/you-tube-presentation-data.services';

@Component({
    selector: 'app-you-tube-presentation',
    templateUrl: './you-tube-presentation.component.html',
    styleUrls: ['./you-tube-presentation.component.scss']
})
export class YouTubePresentationComponent implements OnInit {
    id: string;

    youTubePresentationStyles: YouTubePresentationStyles;

    youTubePresentation: YouTubePresentation;

    constructor(
        public youTubePresentationDataServices: YouTubePresentationDataServices,
        private location: Location,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'] as string;
        });

        this.loadPresentation();
    }
    getBodyStyle() {
        const data = this.youTubePresentation.presentation.LayoutMetadata;
        return {
            'background-color': CssUtility.getColorHex(data['@backgroundColor'])
        };
    }
    getPlaylistStyle() {
        const data = this.youTubePresentation.presentation.LayoutMetadata
            .playlist;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }
    getProseStyle() {
        const data = this.youTubePresentation.presentation.LayoutMetadata.prose;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }
    getTitleStyle() {
        const data = this.youTubePresentation.presentation.LayoutMetadata.title;
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
        this.youTubePresentationDataServices
            .loadVideos(this.id)
            .then((dataOrErrorResponse: any) => {
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

                this.youTubePresentation.videos = dataOrErrorResponse;
            });

        this.youTubePresentationDataServices
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

                this.youTubePresentation.presentation =
                    dataOrErrorResponse.Presentation;

                // if (scope.clientVM) {
                //     scope.clientVM.style.body = this.getBodyStyle();
                // }
                this.youTubePresentationStyles.playlist = this.getPlaylistStyle();
                this.youTubePresentationStyles.prose = this.getProseStyle();
                this.youTubePresentationStyles.title = this.getTitleStyle();
            });
    }
}
