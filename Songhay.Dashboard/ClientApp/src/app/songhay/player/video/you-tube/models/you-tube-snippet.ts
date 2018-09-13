import { YouTubeResourceId } from './you-tube-resource-id';
import { YouTubeThumbnails } from './you-tube-thumbnails';

/**
 * defines YouTube thumb snippet
 *
 * @export
 * @interface YouTubeSnippet
 */
export interface YouTubeSnippet {
    /**
     * channel ID
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    channelId: string;

    /**
     * channel Title
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    channelTitle: string;

    /**
     * publish date
     *
     * @type {Date}
     * @memberof YouTubeSnippet
     */
    publishedAt: Date;

    /**
     * resource ID
     *
     * @type {YouTubeResourceId}
     * @memberof YouTubeSnippet
     */
    resourceId: YouTubeResourceId;

    /**
     * thumbnail metadata
     *
     * @type {YouTubeThumbnails}
     * @memberof YouTubeSnippet
     */
    thumbnails: YouTubeThumbnails;
}
