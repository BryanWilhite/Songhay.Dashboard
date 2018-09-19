import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';

import { YouTubeThumbsNavigationComponent } from './you-tube-thumbs-navigation.component';
import { YouTubeDataService } from '../../services/you-tube-data.service';

describe(YouTubeThumbsNavigationComponent.name, () => {
    const router = jasmine.createSpyObj(Router.name, ['navigate']);
    const service = jasmine.createSpyObj(YouTubeDataService.name, [
        YouTubeDataService.loadChannelMethodName,
        YouTubeDataService.loadChannelSetMethodName,
        YouTubeDataService.loadChannelsIndexMethodName
    ]);
    let component: YouTubeThumbsNavigationComponent;
    let fixture: ComponentFixture<YouTubeThumbsNavigationComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsNavigationComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Router, useValue: router },
                { provide: YouTubeDataService, useValue: service }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsNavigationComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
