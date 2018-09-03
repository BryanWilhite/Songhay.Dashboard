import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { YouTubeScalars } from '../models/you-tube-scalars';
import { AppDataService } from './songhay-app-data.service';

@Injectable()
export class YouTubeDataService extends AppDataService {
    constructor(client: Http) {
        super(client);
    }

    loadChannel(channelId: string): Promise<Response> {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }
        };

        const uri =
            YouTubeScalars.rxYouTubeApiRootUri +
            YouTubeScalars.rxYouTubeApiPlaylistPath +
            channelId;
        const promise = new Promise<Response>(
            super.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }

    loadChannelSet(id) {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }
        };

        const uri =
            YouTubeScalars.rxYouTubeApiRootUri +
            YouTubeScalars.rxYouTubeApiPlaylistsPath +
            id;

        const promise = new Promise<Response>(
            super.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }

    loadChannelsIndex(suffix) {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }
        };

        const uri =
            YouTubeScalars.rxYouTubeApiRootUri +
            YouTubeScalars.rxYouTubeApiPlaylistsIndexPath +
            suffix;

        const promise = new Promise<Response>(
            super.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }

    loadPresentation(id) {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }
        };

        const uri = YouTubeScalars.rxYouTubeApiRootUri + id;

        const promise = new Promise<Response>(
            super.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }

    loadVideos(id) {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }
        };

        const uri =
            YouTubeScalars.rxYouTubeApiRootUri +
            YouTubeScalars.rxYouTubeApiVideosPath +
            id;

        const promise = new Promise<Response>(
            super.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }
}
