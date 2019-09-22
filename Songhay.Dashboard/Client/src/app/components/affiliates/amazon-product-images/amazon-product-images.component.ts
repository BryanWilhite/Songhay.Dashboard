import { Subscription } from 'rxjs';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AmazonDataStore } from 'src/app/services/amazon-data.store';
import { AmazonProduct } from '../../../models/amazon-product';

@Component({
    selector: 'app-amazon-product-images',
    templateUrl: './amazon-product-images.component.html',
    styleUrls: ['./amazon-product-images.component.scss']
})
export class AmazonProductImagesComponent implements OnInit, OnDestroy {
    amazonForm: FormGroup;
    amazonProducts: AmazonProduct[];

    private subscriptions: Subscription[] = [];

    constructor(
        public amazonDataStore: AmazonDataStore,
        private builder: FormBuilder
    ) {}

    ngOnInit() {
        this.amazonForm = this.builder.group({
            asins: ['B004QRKWKQ,B0769XXGXX,B005LKB0IU', [Validators.required]]
        });

        const sub = this.amazonDataStore.serviceData.subscribe(data => {
            this.amazonProducts = data;
        });

        this.subscriptions.push(sub);
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }

    get asins() {
        if (!this.amazonForm) {
            return;
        }
        return this.amazonForm.get('asins');
    }

    getProductImages(): void {
        this.amazonDataStore.loadProducts(this.asins.value as string);
    }
}
