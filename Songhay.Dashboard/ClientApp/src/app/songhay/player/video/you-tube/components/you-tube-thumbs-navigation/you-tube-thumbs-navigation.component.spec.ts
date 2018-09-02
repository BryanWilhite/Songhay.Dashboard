import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YouTubeThumbsNavigationComponent } from './you-tube-thumbs-navigation.component';

describe('YouTubeThumbsNavigationComponent', () => {
  let component: YouTubeThumbsNavigationComponent;
  let fixture: ComponentFixture<YouTubeThumbsNavigationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YouTubeThumbsNavigationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YouTubeThumbsNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
