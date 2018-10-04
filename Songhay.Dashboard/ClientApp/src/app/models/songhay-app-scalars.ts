/**
 * centralizes magic strings in this App
 *
 * @export
 * @class AppScalars
 */
export class AppScalars {
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

    /**
     * location of Amazon products API
     *
     * @static
     * @memberof AppScalars
     */
    static rxAmazonProductRootUri = 'https://songhay-system-staging.azurewebsites.net/api-affiliates/amazon/v1/';

    /**
     * location of the Twitter API
     *
     * @static
     * @type {''}
     * @memberof AppScalars
     */
    static rxTwitterApiRootUri: 'https://songhay-system-staging.azurewebsites.net/api-social/twitter/v1/';

    static setupForJasmine(): void {
        AppScalars.appDataLocation = './_karma_webpack_/assets/data/app.json';
    }
}
