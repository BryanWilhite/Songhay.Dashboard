import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
    CdkDragDrop,
    moveItemInArray,
    transferArrayItem
} from '@angular/cdk-experimental/drag-drop';

import { TwitterItem } from '../../../models/twitter-item';
import { SocialDataService } from '../../../services/social-data.service';

@Component({
    selector: 'app-tweeted-links-builder',
    templateUrl: './tweeted-links-builder.component.html',
    styleUrls: ['./tweeted-links-builder.component.scss']
})
export class TweetedLinksBuilderComponent implements OnInit {
    documentHtml: SafeHtml;
    twitterItemsIn: TwitterItem[];
    twitterItemsOut: TwitterItem[];

    constructor(
        public socialDataService: SocialDataService,
        private sanitizer: DomSanitizer
    ) {}

    ngOnInit() {
        this.socialDataService.twitterItemsLoaded.subscribe(
            (items: TwitterItem[]) =>
                (this.twitterItemsIn = items.map(i => {
                    i.safeHtml = this.linkifyTweet(i.text || i.fullText);
                    return i;
                }))
        );

        this.twitterItemsOut = [];
    }

    drop(event: CdkDragDrop<TwitterItem[]>): void {
        if (event.previousContainer === event.container) {
            if (event.container.id === 'tweetsIn') {
                return;
            }
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
            .map(i => `<p>${this.linkifyTweet(i.fullText || i.text)}</p>`)
            .join('\n');
        console.log({ html });
        this.documentHtml = this.sanitizer.bypassSecurityTrustHtml(html);
    }

    getStatuses(): void {
        this.socialDataService.loadTwitterItems();
    }

    linkifyTweet(tweet: string): SafeHtml {
        const reForHtmlLiteral = /((https?|ftp|file):\/\/[\-A-Z0-9+&@@#\/%?=~_|!:,.;]*[\-A-Z0-9+&@@#\/%=~_|])/gi,
            reForHandle = /[@@]+[A-Za-z0-9-_]+/g,
            reForHashTag = /[#]+[A-Za-z0-9-_]+/g;
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

        return this.sanitizer.bypassSecurityTrustHtml(tweet);
    }

    remove(statusId: number): void {
        console.log({ statusId });
        const currentArray = this.twitterItemsOut,
            targetArray = this.twitterItemsIn,
            currentIndex = this.twitterItemsOut.findIndex(
                i => i.statusID === statusId
            ),
            targetIndex = this.twitterItemsIn.length;

        transferArrayItem(currentArray, targetArray, currentIndex, targetIndex);
    }
}
