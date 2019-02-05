import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SonghayAngularCoreComponent } from './songhay-angular-core.component';

describe('SonghayAngularCoreComponent', () => {
  let component: SonghayAngularCoreComponent;
  let fixture: ComponentFixture<SonghayAngularCoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SonghayAngularCoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SonghayAngularCoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
