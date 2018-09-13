import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { CssUtility } from '../../../../../core/services/songhay-css.utility';
import { Presentation } from '../../../../../core/models/songhay-presentation';

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

        this.youTubePresentationDataServices
            .loadPresentation(this.id)
            .catch(() => {
                console.log(
                    'The expected data is not here.',
                    '[id:',
                    this.id,
                    ']'
                );
                if (this.id) {
                    this.location.replaceState('/not-found');
                }
            });

        this.youTubePresentationDataServices.loadVideos(this.id).catch(() => {
            console.log('The expected data is not here.', '[id:', this.id, ']');
            if (this.id) {
                this.location.replaceState('/not-found');
            }
        });

        this.youTubePresentationDataServices.presentationLoaded.subscribe(
            json => {
                this.youTubePresentation.presentation = json as Presentation;

                // if (scope.clientVM) {
                //     scope.clientVM.style.body = this.getBodyStyle();
                // }
                this.youTubePresentationStyles.playlist = this.getPlaylistStyle();
                this.youTubePresentationStyles.prose = this.getProseStyle();
                this.youTubePresentationStyles.title = this.getTitleStyle();
            }
        );

        this.youTubePresentationDataServices.videosLoaded.subscribe(json => {
            this.youTubePresentation.videos = json as {}[];
        });
    }

    getBodyStyle(): {} {
        const data = this.youTubePresentation.presentation.layoutMetadata;
        return {
            'background-color': CssUtility.getColorHex(data['@backgroundColor'])
        };
    }

    getPlaylistStyle(): {} {
        const data = this.youTubePresentation.presentation.layoutMetadata
            .playlist;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }

    getProseStyle(): {} {
        const data = this.youTubePresentation.presentation.layoutMetadata.prose;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color'])
        };
    }

    getTitleStyle(): {} {
        const data = this.youTubePresentation.presentation.layoutMetadata.title;
        return {
            'background-color': CssUtility.getColorHex(
                data['@backgroundColor']
            ),
            color: CssUtility.getColorHex(data['@color']),
            display: CssUtility.getPixelValue(data['@display'])
        };
    }
}
