import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-amazon-product-images',
    templateUrl: './amazon-product-images.component.html',
    styleUrls: ['./amazon-product-images.component.scss']
})
export class AmazonProductImagesComponent implements OnInit {
    amazonForm: FormGroup;

    constructor(private builder: FormBuilder) {}

    ngOnInit() {
        this.amazonForm = this.builder.group({
            asins: ['B004QRKWKQ,B005Q0E0XC,B005LKB0IU', [Validators.required]]
        });
    }

    get asins() {
        return this.amazonForm.get('asins');
    }

    getProductImages(): void {
        console.log({ asins: this.asins });
    }
}
