import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SonghayPlayerVideoComponent } from './songhay-player-video.component';

describe('SonghayPlayerVideoComponent', () => {
  let component: SonghayPlayerVideoComponent;
  let fixture: ComponentFixture<SonghayPlayerVideoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SonghayPlayerVideoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SonghayPlayerVideoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
