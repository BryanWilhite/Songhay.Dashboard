import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioFeedComponent } from './studio-feed.component';

describe('StudioFeedComponent', () => {
    let component: StudioFeedComponent;
    let fixture: ComponentFixture<StudioFeedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedComponent],
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
