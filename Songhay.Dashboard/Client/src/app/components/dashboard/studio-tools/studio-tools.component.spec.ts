import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioToolsComponent } from './studio-tools.component';

describe(StudioToolsComponent.name, () => {
  let component: StudioToolsComponent;
  let fixture: ComponentFixture<StudioToolsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudioToolsComponent ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StudioToolsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
