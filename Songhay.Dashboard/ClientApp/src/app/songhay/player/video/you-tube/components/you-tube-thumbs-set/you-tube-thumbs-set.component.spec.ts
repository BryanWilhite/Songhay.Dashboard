import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';
import { DataServiceMock } from '../../../../../../mocks/data-service.mock';

import { YouTubeScalars } from '../../models/you-tube-scalars';
import { YouTubeDataService } from '../../services/you-tube-data.service';
import { YouTubeThumbsSetComponent } from './you-tube-thumbs-set.component';

describe('YouTubeThumbsSetComponent', () => {
    const location = jasmine.createSpyObj(Location.name, ['replaceState']);
    let component: YouTubeThumbsSetComponent;
    let fixture: ComponentFixture<YouTubeThumbsSetComponent>;

    YouTubeScalars.setupForJasmine();

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsSetComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Location, useValue: location },
                { provide: YouTubeDataService, useClass: DataServiceMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsSetComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
