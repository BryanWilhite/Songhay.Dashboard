import { of } from 'rxjs';

import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SyndicationFeed } from 'songhay/core/models/syndication-feed';
import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

import { StudioVersionsComponent } from './studio-versions.component';

describe(StudioVersionsComponent.name, () => {
    const dashboardDataStoreMock = {
        loadAppData: () => {},
        serviceData: of(new Map<string, SyndicationFeed>())
    };

    let component: StudioVersionsComponent;
    let fixture: ComponentFixture<StudioVersionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioVersionsComponent],
            providers: [
                { provide: DashboardDataStore, useValue: dashboardDataStoreMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioVersionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
