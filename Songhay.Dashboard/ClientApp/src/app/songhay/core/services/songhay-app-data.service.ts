import { Injectable } from '@angular/core';
import { Http, Response, RequestOptionsArgs } from '@angular/http';
import 'rxjs/add/operator/toPromise';

/**
 * default data service for this App
 *
 * @export
 * @class AppDataService
 */
@Injectable()
export class AppDataService {
    /**
     *Creates an instance of @type {AppDataService}.
     * @param {Http} client
     * @memberof AppDataService
     */
    constructor(private client: Http) {
        this.initializeLoadState();
    }

    /**
     * Returns true when the last API promise is rejected.
     *
     * @type {boolean}
     * @memberof BlogEntriesService
     */
    isError: boolean;

    /**
     * Returns true when the last API call loaded data
     * without any errors.
     *
     * @type {boolean}
     * @memberof BlogEntriesService
     */
    isLoaded: boolean;

    /**
     * Returns true when the API call is promising.
     *
     * @type {boolean}
     * @memberof BlogEntriesService
     */
    isLoading: boolean;

    /**
     * gets the executor
     * for the constructor of @type {Promise}
     *
     * @param {string} url
     * @param {(response: Response, reject?: any) => void} [rejectionExecutor]
     * @returns
     * @memberof AppDataService
     */
    getExecutor(
        url: string,
        rejectionExecutor?: (response: Response, reject?: any) => void,
        requestArgs?: RequestOptionsArgs
    ) {
        const executor = (
            resolve: (Response) => void,
            reject: (any) => void
        ) => {
            this.client
                .get(url, requestArgs)
                .toPromise()
                .then(
                    responseOrVoid => {
                        const response = responseOrVoid as Response;
                        if (!response) {
                            reject('response is not truthy.');
                            return;
                        }

                        if (rejectionExecutor) {
                            rejectionExecutor(response, reject);
                        }

                        this.isLoaded = true;
                        this.isLoading = false;

                        resolve(responseOrVoid);
                    },
                    error => {
                        this.isError = true;
                        this.isLoaded = false;
                        reject(error);
                    }
                );
        };
        return executor;
    }

    /**
     * initializes App data-loading state
     *
     * @memberof AppDataService
     */
    initializeLoadState(): void {
        this.isError = false;
        this.isLoaded = false;
        this.isLoading = false;
    }

    /**
     *
     *
     * @template TFromJson
     * @param {string} uri
     * @param {(json: TFromJson) => void} responseAction
     * @param {RequestOptionsArgs} [requestArgs]
     * @returns {Promise<Response>}
     * @memberof AppDataService
     */
    loadJson<TFromJson>(
        uri: string,
        responseAction: (json: TFromJson) => void,
        requestArgs?: RequestOptionsArgs
    ): Promise<Response> {
        if (this.isLoading) {
            return Promise.reject(
                'WARNING: previous JSON loading operation is still in progress.'
            );
        }

        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as TFromJson;

            if (!data) {
                reject('response JSON data is not truthy.');
                return;
            }

            responseAction(data);
        };

        const promise = new Promise<Response>(
            this.getExecutor(uri, rejectionExecutor, requestArgs)
        );

        return promise;
    }
}