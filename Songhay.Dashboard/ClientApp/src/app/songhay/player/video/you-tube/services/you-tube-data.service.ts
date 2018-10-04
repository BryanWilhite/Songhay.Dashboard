import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';

import { YouTubeScalars } from '../models/you-tube-scalars';
import { AppDataService } from '../../../../core/services/songhay-app-data.service';
import { YouTubeItem } from '../models/you-tube-item';

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
        this.initialize();
    }

    /**
     * gets @type {YouTubeItem[]}
     * from JSON of the shape { items: [] } }
     *
     * @static
     * @param {{}} json
     * @returns {YouTubeItem[]}
     * @memberof YouTubeDataService
     */
    static getItems(json: {}): YouTubeItem[] {
        const items = json['items'] as YouTubeItem[];
        return items;
    }

    /**
     * gets @type {Map<string, YouTubeItem[]>}
     * from JSON of the shape { set: [{ items: [] }] }
     *
     * @static
     * @param {{}} json
     * @returns {Map<string, YouTubeItem[]>}
     * @memberof YouTubeDataService
     */
    static getItemsMap(json: {}): Map<string, YouTubeItem[]> {
        const set = Array.from(json['set']).filter(i => {
            const test = i && i['items'];
            if (!test) {
                console.warn({
                    component: YouTubeDataService.name,
                    message: 'getItemsMap(): item filtered out',
                    itemFilteredOut: i
                });
            }
            return test;
        });
        return new Map(
            set.map(o => {
                const items = YouTubeDataService.getItems(o);
                const key = items[0].snippet.channelTitle;
                return [key, items] as [string, YouTubeItem[]];
            })
        );
    }

    /**
     * loads YouTube channel
     *
     * @param {string} channelId
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannel(channelId: string): Promise<Response> {
        this.initialize();

        const uri = `${YouTubeScalars.rxYouTubeApiRootUri}${
            YouTubeScalars.rxYouTubeApiPlaylistPath
        }${channelId}`;

        return this.loadJson<{}>(uri, json => this.channelLoaded.emit(json));
    }

    /**
     * loads YouTube channel set
     *
     * @param {string} id
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannelSet(suffix: string, id: string): Promise<Response> {
        this.initialize();

        const uri = `${YouTubeScalars.rxYouTubeApiRootUri}${
            YouTubeScalars.rxYouTubeApiPlaylistsPath
        }${suffix}/${id}`;

        return this.loadJson<{}>(uri, json => this.channelSetLoaded.emit(json));
    }

    /**
     * loads YouTube channels index
     *
     * @param {string} suffix
     * @returns {Promise<Response>}
     * @memberof YouTubeDataService
     */
    loadChannelsIndex(suffix: string): Promise<Response> {
        this.initialize();

        const uri = `${YouTubeScalars.rxYouTubeApiRootUri}${
            YouTubeScalars.rxYouTubeApiPlaylistsIndexPath
        }${suffix}`;

        return this.loadJson<{}>(uri, json =>
            this.channelsIndexLoaded.emit(json)
        );
    }

    private initialize(): void {
        super.initializeLoadState();
    }
}
