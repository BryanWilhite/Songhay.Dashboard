import * as _ from 'lodash';
import * as moment from 'moment';

import { Component, OnInit } from '@angular/core';

import { YouTubeScalars } from '../../models/you-tube-scalars';
import { YouTubeThumbs } from '../../models/you-tube-thumbs';

@Component({
    selector: 'app-you-tube-thumbs',
    templateUrl: './you-tube-thumbs.component.html',
    styleUrls: ['./you-tube-thumbs.component.scss']
})
export class YouTubeThumbsComponent implements OnInit {
    disableDefaultSort: boolean;
    thumbsAnimationDuration: number;
    thumbsData: YouTubeThumbs;
    thumbsHeaderLevel: number;
    thumbsTitle: string;
    thumbsTitleData: {};

    constructor() {
        this.initialize();
    }

    ngOnInit() {}

    getDuration(contentDetails) {
        if (!contentDetails) {
            console.log(
                'getDuration()',
                'The expected content details are not here.'
            );
            return;
        }
        if (!contentDetails.duration) {
            console.log(
                'getDuration()',
                'The expected content details duration is not here.'
            );
            return;
        }
        const duration = moment.duration(contentDetails.duration);
        const hours = this.getLeadingZeroOrDefault(duration.hours());
        const minutes = this.getLeadingZeroOrDefault(duration.minutes());
        const seconds = this.getLeadingZeroOrDefault(duration.seconds());
        let display = (hours + ':' + minutes + ':' + seconds).replace(
            /^0[0:]*/g,
            ''
        );
        if (display.length === 1) {
            display = '0:0' + display;
        }
        if (display.length > 1 && display.length < 3) {
            display = '0:' + display;
        }
        return display;
    }

    getLeadingZeroOrDefault(n) {
        return ('0' + n).slice(-2);
    }

    getPublishedAt(snippet) {
        const publishedAt = moment(snippet.publishedAt).fromNow();
        return publishedAt;
    }

    getThumbCaption(item) {
        const kind = item.kind;
        const snippet = item.snippet;
        const limit = 60;
        const title = snippet.title;
        const caption =
            title.length > limit ? title.substring(0, limit) + '…' : title;
        const videoId =
            kind === 'youtube#video' ? item.id : snippet.resourceId.videoId;
        return (
            '<a href="' +
            this.getYouTubeHref(item) +
            '" title="' +
            title +
            '" target="_blank">' +
            caption +
            '</a>'
        );
    }

    getThumbsTitle() {
        const snippet0 = this.thumbsData.items[0].snippet;
        const channelHref =
            'https://www.youtube.com/channel/' + snippet0.channelId;
        const getTitle = function() {
            if (this.thumbsTitleData) {
                return this.thumbsTitleData;
            }

            if (!this.thumbsTitle) {
                return (
                    '<a href="' +
                    channelHref +
                    '" target="_blank" title="view Channel on YouTube">' +
                    snippet0.channelTitle +
                    '</a>'
                );
            }

            return this.thumbsTitle;
        };

        let title = getTitle();

        const level = this.thumbsHeaderLevel ? this.thumbsHeaderLevel : '2';
        const h = 'h' + level;
        title = '<' + h + '>' + title + '</' + h + '>';
        return title;
    }

    getYouTubeHref(item) {
        const kind = item.kind;
        const snippet = item.snippet;
        const videoId =
            kind === 'youtube#video' ? item.id : snippet.resourceId.videoId;

        if (!videoId) {
            console.log(
                'getYouTubeHref()',
                'The expected video ID is not here.'
            );
            return;
        }

        return YouTubeScalars.rxYouTubeWatchRootUri + videoId;
    }

    initialize() {
        if (this.disableDefaultSort) {
            return;
        }
        console.log('directiveVM.initialize()', 'sorting thumbs data...');
        // this.thumbsData.items = _(this.thumbsData.items)
        //     .orderBy(['snippet.publishedAt'], ['desc'])
        //     .value();
    }

    slideThumbs(direction) {
        console.log('slideThumbs() called...');
        const wrapperContainer = $('.video.thumbs-container', element);
        const wrapperContainerWidth = wrapperContainer.width();
        const blockWrapper = $('> div', wrapperContainer);
        const duration = this.thumbsAnimationDuration
            ? this.thumbsAnimationDuration
            : 500; // default slide duration in ms
        const wrapperLeft = parseInt(blockWrapper.css('left'), 10);
        const cannotSlideLeft = function() {
            console.log('cannotSlideLeft() called...');
            const snippet0 = this.thumbsData.items[0].snippet;
            const fixedBlockWidth =
                parseInt(snippet0.thumbnails.medium.width, 10) + 4;
            const blocks = $('> span', blockWrapper);
            const totalWidth = fixedBlockWidth * blocks.length;
            const slideLeftLength =
                Math.abs(wrapperLeft) + wrapperContainerWidth;
            const test = slideLeftLength >= totalWidth;
            console.log(
                'test:',
                test,
                'slideLeftLength:',
                slideLeftLength,
                'totalWidth:',
                totalWidth,
                'fixedBlockWidth:',
                fixedBlockWidth
            );
            return test;
        };
        const getSlideRightLength = function() {
            console.log('getSlideRightLength() called...');
            const l = Math.abs(wrapperLeft);
            const length =
                l > wrapperContainerWidth ? wrapperContainerWidth : l;
            console.log('length:', length);
            return length;
        };
        console.log(
            'blockWrapper:',
            blockWrapper,
            'wrapperContainer:',
            wrapperContainer,
            'wrapperContainerWidth:',
            wrapperContainerWidth,
            'wrapperLeft:',
            wrapperLeft
        );

        switch (direction) {
            case 'left':
                if (cannotSlideLeft()) {
                    return;
                }
                blockWrapper.animate(
                    {
                        left: '-=' + wrapperContainer.width()
                    },
                    duration
                );
                break;

            case 'right':
                if (blockWrapper.position().left >= 0) {
                    return;
                }
                blockWrapper.animate(
                    {
                        left: '+=' + getSlideRightLength()
                    },
                    duration
                );
                break;
        }
    }
}
