import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';

import { YouTubePresentationDataServices } from '../../services/you-tube-presentation-data.services';
import { YouTubePresentationComponent } from './you-tube-presentation.component';

describe(YouTubePresentationComponent.name, () => {
    const location = jasmine.createSpyObj(Location.name, ['replaceState']);
    const service = jasmine.createSpyObj(YouTubePresentationDataServices.name, [
        YouTubePresentationDataServices.loadPresentationMethodName,
        YouTubePresentationDataServices.loadVideosMethodName
    ]);
    let component: YouTubePresentationComponent;
    let fixture: ComponentFixture<YouTubePresentationComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubePresentationComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Location, useValue: location },
                { provide: YouTubePresentationDataServices, useValue: service }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubePresentationComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
