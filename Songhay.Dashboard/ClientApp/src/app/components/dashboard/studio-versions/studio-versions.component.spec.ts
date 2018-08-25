import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioVersionsComponent } from './studio-versions.component';

describe('StudioVersionsComponent', () => {
    let component: StudioVersionsComponent;
    let fixture: ComponentFixture<StudioVersionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioVersionsComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioVersionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
