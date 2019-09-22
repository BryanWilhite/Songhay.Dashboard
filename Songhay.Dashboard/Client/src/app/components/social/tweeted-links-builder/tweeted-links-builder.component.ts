import { Subscription } from 'rxjs';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
    CdkDragDrop,
    moveItemInArray,
    transferArrayItem
} from '@angular/cdk/drag-drop';

import { SocialDataStore } from 'src/app/services/social-data.store';

import { TwitterItem } from '../../../models/twitter-item';

@Component({
    selector: 'app-tweeted-links-builder',
    templateUrl: './tweeted-links-builder.component.html',
    styleUrls: ['./tweeted-links-builder.component.scss']
})
export class TweetedLinksBuilderComponent implements OnInit, OnDestroy {
    documentHtml: SafeHtml;
    twitterItemsIn: TwitterItem[];
    twitterItemsOut: TwitterItem[];

    private subscriptions: Subscription[] = [];

    static remove(
        statusId: number,
        twitterItemsIn: TwitterItem[],
        twitterItemsOut: TwitterItem[]
    ): void {
        console.log({ statusId });
        const currentArray = twitterItemsOut;
        const targetArray = twitterItemsIn;
        const currentIndex = twitterItemsOut.findIndex(
            i => i.statusID === statusId
        );
        const targetIndex = twitterItemsIn.length;

        transferArrayItem(currentArray, targetArray, currentIndex, targetIndex);
    }

    constructor(
        public socialDataStore: SocialDataStore,
        private sanitizer: DomSanitizer
    ) { }

    ngOnInit() {
        const sub = this.socialDataStore.serviceData.subscribe(
            (items: TwitterItem[]) =>
                (this.twitterItemsIn = items.map((t, i) => {
                    t.ordinal = i;
                    t.safeHtml = this.sanitizer.bypassSecurityTrustHtml(
                        this.linkifyTweet(t.text || t.fullText)
                    );
                    return t;
                }))
        );

        this.subscriptions.push(sub);

        this.documentHtml = '';
        this.twitterItemsOut = [];
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }

    drop(event: CdkDragDrop<TwitterItem[]>): void {
        if (event.previousContainer === event.container) {
            // if (event.container.id === 'tweetsIn') {
            //     return;
            // }
            moveItemInArray(
                event.container.data,
                event.previousIndex,
                event.currentIndex
            );
        } else {
            transferArrayItem(
                event.previousContainer.data,
                event.container.data,
                event.previousIndex,
                event.currentIndex
            );
        }

        const html = this.twitterItemsOut
            .map(i =>
                [
                    `<p class="tweet" data-status-id="${i.statusID}">`,
                    `    <a href="${this.getUserUri(i)}" target="_blank">`,
                    `        <img`,
                    `            alt="${i.user.name} [${
                    i.user.screenNameResponse
                    }]"`,
                    `            src="${i.profileImageUrl}" />`,
                    '    </a>',
                    `    ${this.linkifyTweet(i.fullText || i.text)}`,
                    '</p>'
                ].join('\n')
            )
            .join('\n');
        this.documentHtml = this.sanitizer.bypassSecurityTrustHtml(html);
    }

    getStatuses(): void {
        this.documentHtml = '';
        this.twitterItemsIn = [];
        this.twitterItemsOut = [];
        this.socialDataStore.loadTwitterItems();
    }

    getUserUri(item: TwitterItem): string | null {
        if (!item) {
            console.error('The expected Twitter item is not here');
            return null;
        }
        if (!item.user) {
            console.error('The expected Twitter user is not here');
            return null;
        }
        if (item.user.url) { return item.user.url; }
        if (!item.user.name) {
            console.error('The expected Twitter user name is not here');
            return null;
        }
        return `https://twitter.com/${item.user.screenNameResponse}`;
    }

    linkifyTweet(tweet: string): string {
        const reForHtmlLiteral = /((https?|ftp|file):\/\/[\-A-Z0-9+&@@#\/%?=~_|!:,.;]*[\-A-Z0-9+&@@#\/%=~_|])/gi;
        const reForHandle = /[@@]+[A-Za-z0-9-_]+/g;
        const reForHashTag = /[#]+[A-Za-z0-9-_]+/g;

        tweet = tweet
            .replace(reForHtmlLiteral, '<a href="$1" target="_blank">$1</a>')
            .replace(reForHandle, s => {
                const username = s.replace('@@', '');
                return `<a href="http://twitter.com/${username}" target="_blank">${s}</a>`;
            })
            .replace(reForHashTag, s => {
                const tag = s.replace('#', '%23');
                return `<a href="http://twitter.com/search?q='${tag}" target="_blank">${s}</a>`;
            });

        return tweet;
    }
}
