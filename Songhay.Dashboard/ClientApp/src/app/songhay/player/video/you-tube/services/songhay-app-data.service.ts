import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
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
     *Creates an instance of AppDataService.
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
     * for the constructor of Promise
     *
     * @param {string} url
     * @param {(response: Response, reject?: any) => void} [rejectionExecutor]
     * @returns
     * @memberof AppDataService
     */
    getExecutor(
        url: string,
        rejectionExecutor?: (response: Response, reject?: any) => void
    ) {
        const executor = (
            resolve: (Response) => void,
            reject: (any) => void
        ) => {
            this.client
                .get(url)
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
        this.isLoading = true;
    }

    /**
     * loads JSON with the specified URI and ID
     *
     * @template TJsonResponse
     * @param {string} uri
     * @param {string} id
     * @returns {Promise<Response>}
     * @memberof AppDataService
     */
    loadJsonById<TJsonResponse>(uri: string, id: string): Promise<Response> {
        const rejectionExecutor = (response: Response, reject: any) => {
            const data = response.json() as TJsonResponse;

            if (!data) {
                reject('response JSON data is not truthy.');
                return;
            }
        };

        const promise = new Promise<Response>(
            this.getExecutor(uri, rejectionExecutor)
        );

        return promise;
    }
}
