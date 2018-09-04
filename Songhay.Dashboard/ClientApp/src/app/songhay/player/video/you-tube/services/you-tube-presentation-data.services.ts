import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { AppDataService } from '../../../../core/services/songhay-app-data.service';
import { YouTubeScalars } from '../models/you-tube-scalars';

@Injectable()
export class YouTubePresentationDataServices {
    constructor(client: Http) {
        this.presentationDataService = new AppDataService(client);
        this.videosDataService = new AppDataService(client);
    }

    presentationDataService: AppDataService;

    videosDataService: AppDataService;

    loadPresentation(id): Promise<Response> {
        const uri = `${YouTubeScalars.rxYouTubeApiRootUri} ${id}`;
        return this.presentationDataService.loadJson<{}>(uri, json => {});
    }

    loadVideos(id): Promise<Response> {
        const uri = `${YouTubeScalars.rxYouTubeApiRootUri} ${
            YouTubeScalars.rxYouTubeApiVideosPath
        } ${id}`;

        return this.videosDataService.loadJson<{}>(uri, json => {});
    }
}
