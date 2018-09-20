/**
 * centralizes magic strings for YouTube services, components, etc.
 *
 * @export
 * @class YouTubeScalars
 */
export class YouTubeScalars {
    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeApiRootUri =
        'https://songhay-system-player.azurewebsites.net/api/Player/v1/video/youtube/';

    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeApiPlaylistPath = 'playlist/uploads/';

    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeApiPlaylistsPath = 'playlists/';

    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeApiPlaylistsIndexPath = 'playlist/index/';

    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeApiVideosPath = 'videos/';

    /**
     * magic string
     *
     * @static
     * @memberof YouTubeScalars
     */
    static rxYouTubeWatchRootUri = 'https://www.youtube.com/watch?v=';

    static setupForJasmine(): void {
        YouTubeScalars.rxYouTubeApiRootUri = './base/src/assets/data/';
    }
}
