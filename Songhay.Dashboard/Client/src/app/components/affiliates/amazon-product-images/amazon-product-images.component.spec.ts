import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AmazonDataStore } from 'src/app/services/amazon-data.store';

import { AmazonProductImagesComponent } from './amazon-product-images.component';

describe(AmazonProductImagesComponent.name, () => {
    const amazonDataStoreSpy = jasmine.createSpyObj<AmazonDataStore>(AmazonDataStore.name, []);
    const formGroupSpy = jasmine.createSpyObj<FormGroup>(FormGroup.name, ['get']);

    let component: AmazonProductImagesComponent;
    let fixture: ComponentFixture<AmazonProductImagesComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AmazonProductImagesComponent],
            providers: [
                FormBuilder,
                { provide: AmazonDataStore, useValue: amazonDataStoreSpy },
                { provide: FormGroup, useValue: formGroupSpy }
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
