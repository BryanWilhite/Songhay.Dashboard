import { SafeHtml } from '@angular/platform-browser';
import { TwitterUser } from './twitter-user';

export interface TwitterItem {
    profileImageUrl: string;
    createdAt: Date;
    statusID: number;
    text: string | null;
    fullText: string | null;
    ordinal: number;
    safeHtml: SafeHtml;
    source: string;
    user: TwitterUser;
}
