import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Location } from '@angular/common';

import { StudioNavComponent } from './studio-nav.component';

describe(StudioNavComponent.name, () => {
    let component: StudioNavComponent;
    let fixture: ComponentFixture<StudioNavComponent>;

    const location  = jasmine.createSpyObj(`${StudioNavComponent.name}-location`, ['back']);

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [StudioNavComponent]
        })
            .overrideComponent(StudioNavComponent, {
                set: { providers: [{ provide: Location, useValue: location }] }
            })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(StudioNavComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
