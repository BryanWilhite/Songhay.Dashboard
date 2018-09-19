import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DataServiceMock } from '../../../mocks/data-service.mock';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { StudioFeedComponent } from './studio-feed.component';
import { AppScalars } from '../../../models/songhay-app-scalars';

describe(StudioFeedComponent.name, () => {
    let component: StudioFeedComponent;
    let fixture: ComponentFixture<StudioFeedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedComponent],
            providers: [
                { provide: DashboardDataService, useClass: DataServiceMock }
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
