import { of } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SyndicationFeed } from 'songhay/core/models/syndication-feed';

import { AppScalars } from '../../../models/songhay-app-scalars';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

import { StudioFeedComponent } from './studio-feed.component';

describe(StudioFeedComponent.name, () => {
    const dashboardDataStoreMock = {
        loadAppData: () => {},
        serviceData: of(new Map<string, SyndicationFeed>())
    };

    let component: StudioFeedComponent;
    let fixture: ComponentFixture<StudioFeedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedComponent],
            providers: [
                { provide: DashboardDataStore, useValue: dashboardDataStoreMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioFeedComponent);
        component = fixture.componentInstance;
        component.feedName = AppScalars.feedNameCodePen;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
