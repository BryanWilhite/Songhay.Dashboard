import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { YouTubeDataServiceMock } from '@songhay/player-video-you-tube';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';
import { YouTubeDataService } from '@songhay/player-video-you-tube';

import { DashboardComponent } from './dashboard.component';

describe(DashboardComponent.name, () => {
    const dashboardDataStore = jasmine.createSpyObj<DashboardDataStore>(
        DashboardDataStore.name,
        ['loadAppData']
    );
    let component: DashboardComponent;
    let fixture: ComponentFixture<DashboardComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [DashboardComponent],
            providers: [
                {
                    provide: DashboardDataStore,
                    useValue: dashboardDataStore
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
