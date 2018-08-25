import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioLogoComponent } from './studio-logo.component';

describe('StudioLogoComponent', () => {
    let component: StudioLogoComponent;
    let fixture: ComponentFixture<StudioLogoComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioLogoComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioLogoComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
