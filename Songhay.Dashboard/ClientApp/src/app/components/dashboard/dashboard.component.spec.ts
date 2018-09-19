import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDataService } from '../../services/dashboard-data.service';
import { DashboardComponent } from './dashboard.component';

describe('DashboardComponent', () => {
    const service = jasmine.createSpyObj(DashboardDataService.name, [
        DashboardDataService.loadAppDataMethodName
    ]);
    let component: DashboardComponent;
    let fixture: ComponentFixture<DashboardComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [DashboardComponent],
            providers: [{ provide: DashboardDataService, useValue: service }],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(DashboardComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
