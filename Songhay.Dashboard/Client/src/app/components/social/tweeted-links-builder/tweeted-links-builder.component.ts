import { Subscription } from 'rxjs';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
    CdkDragDrop,
    moveItemInArray,
    transferArrayItem
} from '@angular/cdk/drag-drop';

import { SocialDataStore } from 'src/app/services/social-data.store';

import { TwitterMarkingUtility } from 'src/app/utilities/twitter-marking.utility';
import { TwitterItem } from 'src/app/models/twitter-item';

@Component({
    selector: 'app-tweeted-links-builder',
    templateUrl: './tweeted-links-builder.component.html',
    styleUrls: ['./tweeted-links-builder.component.scss']
})
export class TweetedLinksBuilderComponent implements OnInit, OnDestroy {
    documentMarkdown: SafeHtml;
    documentMarkup: SafeHtml;
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
                    t.markup = this.sanitizer.bypassSecurityTrustHtml(
                        TwitterMarkingUtility.getMarkupLinks(t.text || t.fullText)
                    );
                    return t;
                }))
        );

        this.subscriptions.push(sub);

        this.documentMarkdown = '';
        this.documentMarkup = '';
        this.twitterItemsOut = [];
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }

    drop(event: CdkDragDrop<TwitterItem[]>): void {
        if (event.previousContainer === event.container) {
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

        const markings: { markdown: string; markup: string; }[] =
            this.twitterItemsOut.map(i => ({
                markdown: TwitterMarkingUtility.getMarkdown(i),
                markup: TwitterMarkingUtility.getMarkup(i)
            }));

        const markdown = markings.map(i => i.markdown).join('\n');
        this.documentMarkdown = this.sanitizer.bypassSecurityTrustHtml(markdown);

        const markup = markings.map(i => i.markup).join('\n');
        this.documentMarkup = this.sanitizer.bypassSecurityTrustHtml(markup);
    }

    getStatuses(): void {
        this.documentMarkdown = '';
        this.documentMarkup = '';
        this.twitterItemsIn = [];
        this.twitterItemsOut = [];
        this.socialDataStore.loadTwitterItems();
    }
}
