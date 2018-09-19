import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteMock } from '../../../../../core/mocks/activated-route-mock';
import { YouTubeDataServiceMock } from '../../mocks/you-tube-data-service.mock';

import { YouTubeThumbsNavigationComponent } from './you-tube-thumbs-navigation.component';
import { YouTubeDataService } from '../../services/you-tube-data.service';

describe(YouTubeThumbsNavigationComponent.name, () => {
    const router = jasmine.createSpyObj(Router.name, ['navigate']);
    let component: YouTubeThumbsNavigationComponent;
    let fixture: ComponentFixture<YouTubeThumbsNavigationComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsNavigationComponent],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteMock },
                { provide: Router, useValue: router },
                { provide: YouTubeDataService, useClass: YouTubeDataServiceMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsNavigationComponent);
        component = fixture.componentInstance;

        component.channelsIndexName = 'songhay';
        component.channelTitle = 'channel title';
        component.dataForYouTubeSetIndex = null;
        component.id = '999';

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
