import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeScalars } from '../../../songhay/player/video/you-tube/models/you-tube-scalars';
import { StudioComponent } from './studio.component';

describe('StudioComponent', () => {
    let component: StudioComponent;
    let fixture: ComponentFixture<StudioComponent>;

    YouTubeScalars.setupForJasmine();

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioComponent],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
