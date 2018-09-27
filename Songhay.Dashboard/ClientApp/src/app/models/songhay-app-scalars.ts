/**
 * centralizes magic strings in this App
 *
 * @export
 * @class AppScalars
 */
export class AppScalars {
    /**
     * location of Amazon products API
     *
     * @static
     * @memberof AppScalars
     */
    static amazonProductRootUri = 'https://songhay-system-staging.azurewebsites.net/api-affiliates/amazon/v1/';

    /**
     * location of App data
     *
     * @static
     * @memberof AppScalars
     */
    static appDataLocation = './assets/data/app.json';

    /**
     * syndication feed name
     *
     * @static
     * @memberof AppScalars
     */
    static feedNameCodePen = 'codepen';

    /**
     * syndication feed name
     *
     * @static
     * @memberof AppScalars
     */
    static feedNameFlickr = 'flickr';

    /**
     * syndication feed name
     *
     * @static
     * @memberof AppScalars
     */
    static feedNameGitHub = 'github';

    /**
     * syndication feed name
     *
     * @static
     * @memberof AppScalars
     */
    static feedNameStackOverflow = 'stackoverflow';

    /**
     * syndication feed name
     *
     * @static
     * @memberof AppScalars
     */
    static feedNameStudio = 'studio';

    static setupForJasmine(): void {
        AppScalars.appDataLocation = './base/src/assets/data/app.json';
    }
}
