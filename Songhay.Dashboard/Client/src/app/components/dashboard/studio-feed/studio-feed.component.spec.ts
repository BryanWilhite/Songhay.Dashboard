import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

import { StudioFeedComponent } from './studio-feed.component';
import { AppScalars } from '../../../models/songhay-app-scalars';

describe(StudioFeedComponent.name, () => {
    const dashboardDataStore = jasmine.createSpyObj<DashboardDataStore>(
        DashboardDataStore.name,
        ['loadAppData']
    );

    let component: StudioFeedComponent;
    let fixture: ComponentFixture<StudioFeedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedComponent],
            providers: [
                { provide: DashboardDataStore, useValue: dashboardDataStore }
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
