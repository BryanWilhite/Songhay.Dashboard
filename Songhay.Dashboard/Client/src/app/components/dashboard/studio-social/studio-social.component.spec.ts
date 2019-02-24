import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioSocialComponent } from './studio-social.component';

describe(StudioSocialComponent.name, () => {
    let component: StudioSocialComponent;
    let fixture: ComponentFixture<StudioSocialComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioSocialComponent],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioSocialComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
