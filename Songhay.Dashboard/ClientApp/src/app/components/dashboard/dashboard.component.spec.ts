import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { YouTubeDataServiceMock } from '../../songhay/player/video/you-tube/mocks/you-tube-data-service.mock';

import { DashboardDataService } from '../../services/dashboard-data.service';
import { YouTubeDataService } from '../../songhay/player/video/you-tube/services/you-tube-data.service';
import { DashboardComponent } from './dashboard.component';

describe(DashboardComponent.name, () => {
    const dashboardDataService = jasmine.createSpyObj(
        DashboardDataService.name,
        [DashboardDataService.loadAppDataMethodName]
    );
    let component: DashboardComponent;
    let fixture: ComponentFixture<DashboardComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [DashboardComponent],
            providers: [
                {
                    provide: DashboardDataService,
                    useValue: dashboardDataService
                },
                {
                    provide: YouTubeDataService,
                    useClass: YouTubeDataServiceMock
                }
            ],
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
