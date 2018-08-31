import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioFeedFlickrComponent } from './studio-feed-flickr.component';

describe('StudioFeedFlickrComponent', () => {
    let component: StudioFeedFlickrComponent;
    let fixture: ComponentFixture<StudioFeedFlickrComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioFeedFlickrComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioFeedFlickrComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
