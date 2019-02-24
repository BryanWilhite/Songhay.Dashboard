import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DataServiceMock } from '../../../mocks/data-service.mock';

import { AmazonProductImagesComponent } from './amazon-product-images.component';
import { AmazonDataService } from '../../../services/amazon-data.service';

describe(AmazonProductImagesComponent.name, () => {
    const formGroup = jasmine.createSpyObj(FormGroup.name, ['get']);

    let component: AmazonProductImagesComponent;
    let fixture: ComponentFixture<AmazonProductImagesComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AmazonProductImagesComponent],
            providers: [
                FormBuilder,
                {provide: AmazonDataService, useClass: DataServiceMock },
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
