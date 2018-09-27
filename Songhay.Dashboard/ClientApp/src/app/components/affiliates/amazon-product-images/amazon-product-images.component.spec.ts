import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AmazonProductImagesComponent } from './amazon-product-images.component';

describe('AmazonProductImagesComponent', () => {
    const builder = {
        group: function() {
            return {
                get: (path: Array<string | number> | string) => {
                    console.log('yup');
                }
            };
        }
    };

    let component: AmazonProductImagesComponent;
    let fixture: ComponentFixture<AmazonProductImagesComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AmazonProductImagesComponent],
            providers: [{ provide: FormBuilder, useValue: builder }, FormGroup],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AmazonProductImagesComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
