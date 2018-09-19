import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { StudioVersionsComponent } from './studio-versions.component';

describe('StudioVersionsComponent', () => {
    const service = jasmine.createSpyObj(DashboardDataService.name, [
        DashboardDataService.loadAppDataMethodName
    ]);
    let component: StudioVersionsComponent;
    let fixture: ComponentFixture<StudioVersionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioVersionsComponent],
            providers: [{ provide: DashboardDataService, useValue: service }],
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
