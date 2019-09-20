import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

import { StudioVersionsComponent } from './studio-versions.component';

describe(StudioVersionsComponent.name, () => {
    const dashboardDataStore = jasmine.createSpyObj<DashboardDataStore>(
        DashboardDataStore.name,
        ['loadAppData']
    );

    let component: StudioVersionsComponent;
    let fixture: ComponentFixture<StudioVersionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioVersionsComponent],
            providers: [
                { provide: DashboardDataStore, useValue: dashboardDataStore }
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
