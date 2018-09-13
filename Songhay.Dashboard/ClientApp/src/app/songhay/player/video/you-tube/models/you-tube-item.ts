import { YouTubeSnippet } from './you-tube-snippet';

/**
 * defines YouTube thumb item
 *
 * @export
 * @interface YouTubeItem
 */
export interface YouTubeItem {
    id: string;
    kind: string;
    snippet: YouTubeSnippet;
}
