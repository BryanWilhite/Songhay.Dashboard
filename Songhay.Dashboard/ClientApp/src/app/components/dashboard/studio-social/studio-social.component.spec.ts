import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioSocialComponent } from './studio-social.component';

describe('StudioSocialComponent', () => {
    let component: StudioSocialComponent;
    let fixture: ComponentFixture<StudioSocialComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioSocialComponent]
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
