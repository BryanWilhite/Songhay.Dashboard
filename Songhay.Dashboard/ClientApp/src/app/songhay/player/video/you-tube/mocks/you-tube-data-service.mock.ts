import { Output, EventEmitter } from '@angular/core';

export class YouTubeDataServiceMock {
    @Output()
    channelLoaded: EventEmitter<any>;

    @Output()
    channelSetLoaded: EventEmitter<any>;

    @Output()
    channelsIndexLoaded: EventEmitter<any>;

    @Output()
    presentationLoaded: EventEmitter<any>;

    @Output()
    videosLoaded: EventEmitter<any>;

    constructor() {
        this.channelLoaded = new EventEmitter();
        this.channelSetLoaded = new EventEmitter();
        this.channelsIndexLoaded = new EventEmitter();
        this.presentationLoaded = new EventEmitter();
        this.videosLoaded = new EventEmitter();
    }

    loadChannel(id: string): any {
        return { catch(): void {} };
    }

    loadChannelSet(id: string): any {
        return { catch(): void {} };
    }

    loadChannelsIndex(id: string): any {
        return { catch(): void {} };
    }

    loadPresentation(id: string): any {
        return { catch(): void {} };
    }

    loadVideos(id: string): any {
        return { catch(): void {} };
    }
}
