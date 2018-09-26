import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import { AppDataService } from '../../../../core/services/songhay-app-data.service';
import { YouTubeScalars } from '../models/you-tube-scalars';

/**
 * YouTube Presentation data services
 *
 * @export
 * @class YouTubePresentationDataServices
 */
@Injectable()
export class YouTubePresentationDataServices {
    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof YouTubePresentationDataServices
     */
    static loadPresentationMethodName = 'loadPresentation';

    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof YouTubePresentationDataServices
     */
    static loadVideosMethodName = 'loadVideos';

    /**
     * emits event when @member loadPresentation resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof YouTubeDataService
     */
    @Output()
    presentationLoaded: EventEmitter<{}>;

    /**
     * emits event when @member loadVideos resolves
     *
     * @type {EventEmitter<{}>}
     * @memberof YouTubeDataService
     */
    @Output()
    videosLoaded: EventEmitter<{}>;

    /**
     * YouTube presentation data service
     *
     * @type {AppDataService}
     * @memberof YouTubePresentationDataServices
     */
    presentationDataService: AppDataService;

    /**
     * YouTube videos data service
     *
     * @type {AppDataService}
     * @memberof YouTubePresentationDataServices
     */
    videosDataService: AppDataService;

    /**
     * Creates an instance of YouTubePresentationDataServices.
     * @param {Http} client
     * @memberof YouTubePresentationDataServices
     */
    constructor(client: Http) {
        this.presentationDataService = new AppDataService(client);
        this.videosDataService = new AppDataService(client);

        this.presentationLoaded = new EventEmitter();
        this.videosLoaded = new EventEmitter();
    }

    /**
     * loads b-roll Presentation metadata
     *
     * @param {string} id
     * @returns {Promise<Response>}
     * @memberof YouTubePresentationDataServices
     */
    loadPresentation(id: string): Promise<Response> {
        const uri = `${YouTubeScalars.rxYouTubeApiRootUri}${id}`;

        return this.presentationDataService.loadJson<{}>(uri, json =>
            this.presentationLoaded.emit(json)
        );
    }

    /**
     * loads YouTube video data
     *
     * @param {string} id
     * @returns {Promise<Response>}
     * @memberof YouTubePresentationDataServices
     */
    loadVideos(id: string): Promise<Response> {
        const uri = `${YouTubeScalars.rxYouTubeApiRootUri}${
            YouTubeScalars.rxYouTubeApiVideosPath
        }${id}`;

        return this.videosDataService.loadJson<{}>(uri, json =>
            this.videosLoaded.emit(json)
        );
    }
}
