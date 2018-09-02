import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeThumbsComponent } from './you-tube-thumbs.component';

describe('YouTubeThumbsComponent', () => {
  let component: YouTubeThumbsComponent;
  let fixture: ComponentFixture<YouTubeThumbsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YouTubeThumbsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YouTubeThumbsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
