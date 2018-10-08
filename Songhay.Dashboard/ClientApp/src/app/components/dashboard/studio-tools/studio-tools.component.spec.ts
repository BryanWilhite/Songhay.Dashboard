import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioToolsComponent } from './studio-tools.component';

describe('StudioToolsComponent', () => {
  let component: StudioToolsComponent;
  let fixture: ComponentFixture<StudioToolsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudioToolsComponent ]
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
