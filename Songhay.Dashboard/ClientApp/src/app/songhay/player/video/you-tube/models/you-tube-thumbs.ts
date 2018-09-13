/**
 * defines YouTube thumbnail data
 *
 * @export
 * @interface YouTubeThumbs
 */
export interface YouTubeThumbs {
    /**
     * thumb items
     *
     * @memberof YouTubeThumbs
     */
    items: YouTubeItem[];
}

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

/**
 * defines YouTube thumbnail metadata
 *
 * @export
 * @interface YouTubeThumbnails
 */
export interface YouTubeThumbnails {
    /**
     * medium thumbnail
     *
     * @type {{ width: number }}
     * @memberof YouTubeThumbnails
     */
    medium: { width: number };
}

/**
 * defines YouTube resource ID
 *
 * @export
 * @class YouTubeResourceId
 */
export class YouTubeResourceId {
    /**
     * video ID
     *
     * @type {string}
     * @memberof YouTubeResourceId
     */
    videoId: string;
}
