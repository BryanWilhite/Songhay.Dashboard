import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubePresentationComponent } from './you-tube-presentation.component';

describe('YouTubePresentationComponent', () => {
  let component: YouTubePresentationComponent;
  let fixture: ComponentFixture<YouTubePresentationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YouTubePresentationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YouTubePresentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
