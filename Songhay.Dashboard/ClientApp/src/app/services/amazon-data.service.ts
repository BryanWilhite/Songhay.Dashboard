import { EventEmitter, Injectable, Output } from '@angular/core';
import { Http, Response } from '@angular/http';



import { AppDataService } from '../songhay/core/services/songhay-app-data.service';
import { AmazonProduct } from '../models/amazon-product';
import { AppScalars } from '../models/songhay-app-scalars';

/**
 * Amazon data service
 *
 * @export
 * @class AmazonDataService
 * @extends {AppDataService}
 */
@Injectable()
export class AmazonDataService extends AppDataService {
    /**
     * name of method on this class for Jasmine spies
     *
     * @static
     * @memberof DashboardDataService
     */
    static loadProductsMethodName = 'loadProducts';

    /**
     * event emits when loadProducts resolves
     *
     * @type {EventEmitter<AmazonProduct[]>}
     * @memberof AmazonDataService
     */
    productsLoaded: EventEmitter<AmazonProduct[]>;

    /**
     *Creates an instance of AmazonDataService.
     * @param {Http} client
     * @memberof AmazonDataService
     */
    constructor(client: Http) {
        super(client);

        this.productsLoaded = new EventEmitter();
    }

    loadProducts(asins: string): Promise<Response> {
        this.initialize();

        const uri = `${AppScalars.rxAmazonProductRootUri}/items/${asins}`;

        return this.loadJson<{}>(uri, (json, reject) => {
            const products = json as AmazonProduct[];

            if (!products) {
                reject('Amazon products are not truthy.');
                return;
            }

            this.productsLoaded.emit(products);
        });
    }

    private initialize(): void {
        super.initializeLoadState();
    }
}
