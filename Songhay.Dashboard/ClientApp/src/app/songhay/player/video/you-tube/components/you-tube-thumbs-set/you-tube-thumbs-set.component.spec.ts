import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeThumbsSetComponent } from './you-tube-thumbs-set.component';

describe('YouTubeThumbsSetComponent', () => {
    let component: YouTubeThumbsSetComponent;
    let fixture: ComponentFixture<YouTubeThumbsSetComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsSetComponent],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsSetComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
