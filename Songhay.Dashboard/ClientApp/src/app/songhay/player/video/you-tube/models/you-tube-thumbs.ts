/**
 * defines YouTube thumbnail data
 *
 * @export
 * @class YouTubeThumbs
 */
export class YouTubeThumbs {
    /**
     * thumb items
     *
     * @memberof YouTubeThumbs
     */
    items: [
        {
            snippet: {
                channelId: string;
                channelTitle: string;
                publishedAt: Date;
            };
        }
    ];
}
