import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DataServiceMock } from '../../../mocks/data-service.mock';

import { DashboardDataService } from '../../../services/dashboard-data.service';
import { StudioVersionsComponent } from './studio-versions.component';

describe('StudioVersionsComponent', () => {
    let component: StudioVersionsComponent;
    let fixture: ComponentFixture<StudioVersionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioVersionsComponent],
            providers: [
                { provide: DashboardDataService, useClass: DataServiceMock }
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
