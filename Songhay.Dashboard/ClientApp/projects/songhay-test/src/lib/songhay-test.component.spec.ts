import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SonghayTestComponent } from './songhay-test.component';

describe('SonghayTestComponent', () => {
  let component: SonghayTestComponent;
  let fixture: ComponentFixture<SonghayTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SonghayTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SonghayTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
