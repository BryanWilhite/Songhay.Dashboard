import { Injectable } from '@angular/core';

import { AppDataStore } from '@songhay/core';

import { AmazonProduct } from '../models/amazon-product';
import { AppScalars } from '../models/songhay-app-scalars';

@Injectable()
export class AmazonDataStore extends AppDataStore<AmazonProduct[], any> {
    loadProducts(asins: string): void {
        const uri = `${AppScalars.rxAmazonProductRootUri}/items/${asins}`;
        this.load(uri);
    }
}
