import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';
import { DataServiceMock } from '../../../../../../mocks/data-service.mock';

import { YouTubePresentationDataServices } from '../../services/you-tube-presentation-data.services';
import { YouTubePresentationComponent } from './you-tube-presentation.component';

describe(YouTubePresentationComponent.name, () => {
    const location = jasmine.createSpyObj(Location.name, ['replaceState']);
    let component: YouTubePresentationComponent;
    let fixture: ComponentFixture<YouTubePresentationComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubePresentationComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Location, useValue: location },
                { provide: YouTubePresentationDataServices, useClass: DataServiceMock }
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
