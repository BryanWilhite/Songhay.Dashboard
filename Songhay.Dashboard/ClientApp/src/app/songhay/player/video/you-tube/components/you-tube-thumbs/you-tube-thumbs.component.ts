import * as _ from 'lodash';
import * as moment from 'moment';

import {
    Component,
    AfterViewInit,
    ElementRef,
    ViewChild,
    Input
} from '@angular/core';

import { AnimationBuilder, AnimationPlayer } from '@angular/animations';

import { slideAnimations, slideAnimation } from './slide.animation';

import { DomUtility } from '../../../../../core/services/songhay-dom.utility';

import { YouTubeScalars } from '../../models/you-tube-scalars';
import { YouTubeSnippet } from '../../models/you-tube-snippet';
import { YouTubeItem } from '../../models/you-tube-item';

@Component({
    selector: 'rx-you-tube-thumbs',
    templateUrl: './you-tube-thumbs.component.html',
    styleUrls: ['./you-tube-thumbs.component.scss']
})
export class YouTubeThumbsComponent implements AfterViewInit {
    @Input()
    disableDefaultSort: boolean;
    @Input()
    thumbsAnimationDuration: number;
    @Input()
    youTubeItems: YouTubeItem[];
    @Input()
    thumbsHeaderLevel: number;
    @Input()
    thumbsTitle: string;

    @ViewChild('thumbsContainer')
    thumbsContainer: ElementRef;

    private thumbsContainerDiv: HTMLDivElement;
    private thumbsContainerDivWrapper: HTMLDivElement;
    private thumbsContainerDivWrapperStyleDeclaration: CSSStyleDeclaration;
    private players: Map<string, AnimationPlayer>;

    constructor(private animationBuilder: AnimationBuilder) {
        this.initialize();
    }

    ngAfterViewInit(): void {
        this.thumbsContainerDiv = DomUtility.getHtmlElement<HTMLDivElement>(
            this.thumbsContainer
        );

        this.thumbsContainerDivWrapper = this.thumbsContainerDiv
            .firstElementChild as HTMLDivElement;

        this.thumbsContainerDivWrapperStyleDeclaration = DomUtility.getStyleDeclaration(
            this.thumbsContainerDivWrapper
        );

        if (this.thumbsContainerDivWrapperStyleDeclaration) {
            this.thumbsContainerDivWrapperStyleDeclaration.left = `${0}px`;
        }

        this.players = new Map();
    }

    getDuration(contentDetails): string {
        if (!contentDetails) {
            console.warn({
                component: YouTubeThumbsComponent.name,
                warning: 'The expected content details are not here.'
            });
            return;
        }
        if (!contentDetails.duration) {
            console.warn({
                component: YouTubeThumbsComponent.name,
                warning: 'The expected content details duration are not here.'
            });
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
            display = `0:0${display}`;
        }
        if (display.length > 1 && display.length < 3) {
            display = `0:${display}`;
        }
        return display;
    }

    getLeadingZeroOrDefault(n): string {
        return `0${n}`.slice(-2);
    }

    getPublishedAt(snippet: YouTubeSnippet): string {
        const publishedAt = moment(snippet.publishedAt).fromNow();
        return publishedAt;
    }

    getThumbCaption(item: YouTubeItem): HTMLAnchorElement {
        const kind = item.kind;
        const snippet = item.snippet;
        const limit = 60;
        const title = snippet.channelTitle;
        const caption =
            title.length > limit ? title.substring(0, limit) + '…' : title;
        const videoId =
            kind === 'youtube#video' ? item.id : snippet.resourceId.videoId;
        const a = new HTMLAnchorElement();
        a.href = this.getYouTubeHref(item);
        a.target = '_blank';
        a.title = title;
        a.innerText = caption;
        return a;
    }

    getThumbsTitle(): HTMLHeadingElement {
        if (!this.youTubeItems) {
            return;
        }
        if (!this.youTubeItems.length) {
            return;
        }
        const snippet0 = this.youTubeItems[0].snippet;
        const channelHref = `https://www.youtube.com/channel/${
            snippet0.channelId
        }`;
        const getTitle = (): string | HTMLAnchorElement => {
            if (!this.thumbsTitle) {
                const a = new HTMLAnchorElement();
                a.href = channelHref;
                a.target = '_blank';
                a.title = 'view Channel on YouTube';
                a.innerText = snippet0.channelTitle;
                return a;
            }

            return this.thumbsTitle;
        };

        const level = this.thumbsHeaderLevel ? this.thumbsHeaderLevel : 2;
        const h = DomUtility.getHtmlHeadingElement(level);

        const title = getTitle();

        if (title instanceof String) {
            h.innerText = title as string;
        } else {
            h.appendChild(title as HTMLAnchorElement);
        }

        return h;
    }

    getYouTubeHref(item: YouTubeItem): string {
        if (!item) {
            return;
        }
        const kind = item.kind;
        const snippet = item.snippet;
        const videoId =
            kind === 'youtube#video' ? item.id : snippet.resourceId.videoId;

        if (!videoId) {
            console.warn({
                component: YouTubeThumbsComponent.name,
                warning: 'The expected video ID is not here.'
            });
            return;
        }

        return `${YouTubeScalars.rxYouTubeWatchRootUri}${videoId}`;
    }

    slideThumbs(direction: string): void {
        console.log({ direction: direction });

        const duration = this.thumbsAnimationDuration
            ? this.thumbsAnimationDuration
            : 500; // default slide duration in ms
        const wrapperContainerWidth = this.thumbsContainerDiv.clientWidth;
        const style = this.thumbsContainerDivWrapperStyleDeclaration;

        const wrapperLeft = style.left ? parseInt(style.left, 10) : 0;

        const blocks = DomUtility.getHtmlElements(
            this.thumbsContainerDivWrapper.children
        )
            .filter(el => el.localName === 'span')
            .map(el => el as HTMLSpanElement);

        const cannotSlideBack = () => {
            const snippet0 = this.youTubeItems[0].snippet;
            const fixedBlockWidth = snippet0.thumbnails.medium.width + 4;
            const totalWidth = fixedBlockWidth * blocks.length;
            const slideBackLength =
                Math.abs(wrapperLeft) + wrapperContainerWidth;
            const test = slideBackLength >= totalWidth;
            console.log({
                test,
                slideBackLength,
                totalWidth,
                fixedBlockWidth
            });
            return test;
        };
        const cannotSlideForward = () => wrapperLeft >= 0;

        const getSlideForwardLength = function(): number {
            const l = Math.abs(wrapperLeft);
            return l > wrapperContainerWidth ? wrapperContainerWidth : l;
        };

        console.log({
            getSlideRightLength: getSlideForwardLength(),
            wrapperContainerWidth,
            wrapperLeft
        });

        switch (direction) {
            case 'forward':
                if (cannotSlideForward()) {
                    console.warn('cannot slide forward');
                    return;
                }

                const lPlayer = this.getPlayer(
                    slideAnimation.id,
                    {
                        time: `${duration}ms`,
                        x1: wrapperLeft,
                        x2: wrapperLeft + getSlideForwardLength()
                    },
                    this.thumbsContainerDivWrapper
                );
                lPlayer.play();
                break;

            case 'back':
                if (cannotSlideBack()) {
                    console.warn('cannot slide back');
                    return;
                }

                const rPlayer = this.getPlayer(
                    slideAnimation.id,
                    {
                        time: `${duration}ms`,
                        x1: wrapperLeft,
                        x2: wrapperLeft - wrapperContainerWidth
                    },
                    this.thumbsContainerDivWrapper
                );

                rPlayer.play();

                break;
        }
    }

    private getPlayer(
        animationId: string,
        params: { time: string; x1: number; x2: number },
        el: Element,
        elIndex: number = 0
    ): AnimationPlayer {
        const uniqueId = `${animationId}-${el.localName}${elIndex}`;

        if (this.players.has(uniqueId)) {
            this.players.get(uniqueId).destroy();
        }

        const animation = slideAnimations.get(animationId);
        const factory = this.animationBuilder.build(animation);
        const player = factory.create(el, { params: params });

        player.onDestroy(() => console.log(`player ${uniqueId} destroyed`));
        player.onDone(() => {
            console.log(`player ${uniqueId} done`);

            this.thumbsContainerDivWrapperStyleDeclaration.left = `${
                params.x2
            }px`;
        });

        this.players.set(uniqueId, player);

        return player;
    }

    private initialize(): void {
        if (this.disableDefaultSort) {
            return;
        }
        if (!this.youTubeItems) {
            return;
        }
        this.youTubeItems = _(this.youTubeItems)
            .orderBy(['snippet.publishedAt'], ['desc'])
            .value();
    }
}
