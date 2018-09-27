import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AmazonProductImagesComponent } from './amazon-product-images.component';

describe('AmazonProductImagesComponent', () => {
    const formGroup = jasmine.createSpyObj(FormGroup.name, ['get']);

    let component: AmazonProductImagesComponent;
    let fixture: ComponentFixture<AmazonProductImagesComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AmazonProductImagesComponent],
            providers: [
                FormBuilder,
                { provide: FormGroup, useValue: formGroup }
            ],
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
