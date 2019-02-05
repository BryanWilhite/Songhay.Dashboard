import { Presentation } from 'songhay-core/models/presentation';

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
    presentation: Presentation;

    /**
     * video meta-data for Presentation
     *
     * @type {{}[]}
     * @memberof YouTubePresentation
     */
    videos: {}[];
}
