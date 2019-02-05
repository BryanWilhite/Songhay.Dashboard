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
     * channel title
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    channelTitle: string;

    /**
     * description
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    description: string;

    /**
     * localized version of description and title
     *
     * @type {{ description: string; title: string }}
     * @memberof YouTubeSnippet
     */
    localized: { description: string; title: string }

    /**
     * playlist ID
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    playlistId: string;

    /**
     * position
     *
     * @type {number}
     * @memberof YouTubeSnippet
     */
    position: number;

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
     * tags
     *
     * @type {string[]}
     * @memberof YouTubeSnippet
     */
    tags: string[]

    /**
     * thumbnail metadata
     *
     * @type {YouTubeThumbnails}
     * @memberof YouTubeSnippet
     */
    thumbnails: YouTubeThumbnails;

    /**
     * title
     *
     * @type {string}
     * @memberof YouTubeSnippet
     */
    title: string;
}
