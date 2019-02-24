import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioComponent } from './studio.component';

describe(StudioComponent.name, () => {
    let component: StudioComponent;
    let fixture: ComponentFixture<StudioComponent>;

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
