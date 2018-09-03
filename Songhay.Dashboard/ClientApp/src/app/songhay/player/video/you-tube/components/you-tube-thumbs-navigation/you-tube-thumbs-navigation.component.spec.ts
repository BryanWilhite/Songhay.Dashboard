import { NO_ERRORS_SCHEMA } from '@angular/core';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeThumbsNavigationComponent } from './you-tube-thumbs-navigation.component';

describe('YouTubeThumbsNavigationComponent', () => {
    let component: YouTubeThumbsNavigationComponent;
    let fixture: ComponentFixture<YouTubeThumbsNavigationComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsNavigationComponent],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsNavigationComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
