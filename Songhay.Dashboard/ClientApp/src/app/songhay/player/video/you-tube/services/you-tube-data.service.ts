import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { YouTubeScalars } from '../models/you-tube-scalars';
import { AppDataService } from '../../../../core/services/songhay-app-data.service';

/**
 * YouTube data service
 *
 * @export
 * @class YouTubeDataService
 * @extends {AppDataService}
 */
@Injectable()
export class YouTubeDataService extends AppDataService {
    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof YouTubeDataService
     */
    static loadChannelMethodName = 'loadChannel';

    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof YouTubeDataService
     */
    static loadChannelSetMethodName = 'loadChannelSet';

    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof YouTubeDataService
     */
    static loadChannelsIndexMethodName = 'loadChannelsIndex';

    /**
     * emits event when @member loadChannel resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof YouTubeDataService
     */
    @Output()
    channelLoaded: EventEmitter<{}>;

    /**
     * emits event when @member loadChannelSet resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof YouTubeDataService
     */
    @Output()
    channelSetLoaded: EventEmitter<{}>;

    /**
     * emits event when @member loadChannelsIndex resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof YouTubeDataService
     */
    @Output()
    channelsIndexLoaded: EventEmitter<{}>;

    /**
     * Creates an instance of @type {YouTubeDataService}.
     * @param {Http} client
     * @memberof YouTubeDataService
     */
    constructor(client: Http) {
        super(client);

        this.channelLoaded = new EventEmitter();
        this.channelSetLoaded = new EventEmitter();
        this.channelsIndexLoaded = new EventEmitter();
    }

    /**
     * loads YouTube channel
     *
     * @param {string} channelId
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannel(channelId: string): Promise<Response> {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }

            this.channelLoaded.emit(data);
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

    /**
     * loads YouTube channel set
     *
     * @param {string} id
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannelSet(id: string): Promise<Response> {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }

            this.channelSetLoaded.emit(data);
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

    /**
     * loads YouTube channels index
     *
     * @param {string} suffix
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannelsIndex(suffix: string): Promise<Response> {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as {};

            if (!data) {
                reject('raw YouTube data is not truthy.');
                return;
            }

            this.channelsIndexLoaded.emit(data);
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
}
