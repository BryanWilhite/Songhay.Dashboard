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

    initializeLoadState(): void {
        this.isError = false;
        this.isLoaded = false;
        this.isLoading = true;
    }
}
