import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';

import { YouTubeDataService } from '../../services/you-tube-data.service';
import { YouTubeThumbsSetComponent } from './you-tube-thumbs-set.component';

describe('YouTubeThumbsSetComponent', () => {
    const location = jasmine.createSpyObj(Location.name, ['replaceState']);
    const service = jasmine.createSpyObj(YouTubeDataService.name, [
        YouTubeDataService.loadChannelMethodName,
        YouTubeDataService.loadChannelSetMethodName,
        YouTubeDataService.loadChannelsIndexMethodName
    ]);
    let component: YouTubeThumbsSetComponent;
    let fixture: ComponentFixture<YouTubeThumbsSetComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsSetComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Location, useValue: location },
                { provide: YouTubeDataService, useValue: service }
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
