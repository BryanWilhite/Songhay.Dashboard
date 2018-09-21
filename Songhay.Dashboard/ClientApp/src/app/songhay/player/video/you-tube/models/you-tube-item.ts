import { YouTubeSnippet } from './you-tube-snippet';
import { YouTubeContentDetails } from './you-tube-content-details';

/**
 * defines YouTube API item
 * of the specified kind
 *
 * the item is the central JSON shape
 * of this API
 *
 * @export
 * @interface YouTubeItem
 */
export interface YouTubeItem {
    /**
     * entity tag to detect content changes
     *
     * @type {string}
     * @memberof YouTubeItem
     */
    etag: string;

    /**
     * unique ID for item
     *
     * @type {string}
     * @memberof YouTubeItem
     */
    id: string;

    /**
     * the kind of the item
     *
     * the kinds used so far:
     *
     * youtube#video
     * youtube#videoListResponse
     *
     * @type {string}
     * @memberof YouTubeItem
     */
    kind: string;

    /**
     * YouTube API snippet
     *
     * @type {YouTubeSnippet}
     * @memberof YouTubeItem
     */
    snippet: YouTubeSnippet;

    /**
     * YouTube item content details
     *
     * @type {YouTubeContentDetails}
     * @memberof YouTubeItem
     */
    contentDetails: YouTubeContentDetails;
}
