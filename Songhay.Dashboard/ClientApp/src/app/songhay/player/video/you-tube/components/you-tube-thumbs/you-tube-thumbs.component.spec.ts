import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AnimationBuilder } from '@angular/animations';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeThumbsComponent } from './you-tube-thumbs.component';

describe('YouTubeThumbsComponent', () => {
    const animationBuilder = jasmine.createSpyObj(AnimationBuilder.name, [
        'build'
    ]);
    let component: YouTubeThumbsComponent;
    let fixture: ComponentFixture<YouTubeThumbsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [YouTubeThumbsComponent],
            providers: [
                { provide: AnimationBuilder, useValue: animationBuilder }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(YouTubeThumbsComponent);
        component = fixture.componentInstance;

        component.disableDefaultSort = false;
        component.thumbsAnimationDuration = 500;
        component.thumbsData = null;
        component.thumbsHeaderLevel = 1;
        component.thumbsTitle = 'thumbs title';

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
