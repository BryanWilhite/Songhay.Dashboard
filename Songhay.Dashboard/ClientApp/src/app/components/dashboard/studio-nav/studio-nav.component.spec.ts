import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudioNavComponent } from './studio-nav.component';

describe('StudioNavComponent', () => {
  let component: StudioNavComponent;
  let fixture: ComponentFixture<StudioNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudioNavComponent ]
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
