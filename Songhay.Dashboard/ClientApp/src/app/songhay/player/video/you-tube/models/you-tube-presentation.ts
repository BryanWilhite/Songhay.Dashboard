/**
 * defines the JSON, converted from XML
 * for a gen-web Presentation manifest
 *
 * @export
 * @class YouTubePresentation
 */
export class YouTubePresentation {
    /**
     * gen-web Presentation
     *
     * @memberof YouTubePresentation
     */
    presentation: {
        LayoutMetadata: { playlist: {}; prose: string; title: string };
        Title: string;
        Description: { '#text': string };
        Credits: { '#text': string };
    };

    /**
     * video meta-data for Presentation
     *
     * @type {{}[]}
     * @memberof YouTubePresentation
     */
    videos: {}[];
}
