import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { StudioFeedComponent } from './studio-feed.component';

describe('StudioFeedComponent', () => {
    const service = jasmine.createSpyObj(DashboardDataService.name, [
        DashboardDataService.loadAppDataMethodName
    ]);
    let component: StudioFeedComponent;
    let fixture: ComponentFixture<StudioFeedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedComponent],
            providers: [{ provide: DashboardDataService, useValue: service }],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioFeedComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
