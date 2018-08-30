import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioLinksComponent } from './studio-links.component';

describe('StudioLinksComponent', () => {
    let component: StudioLinksComponent;
    let fixture: ComponentFixture<StudioLinksComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioLinksComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioLinksComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});