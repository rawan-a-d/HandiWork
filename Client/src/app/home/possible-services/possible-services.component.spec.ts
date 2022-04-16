import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PossibleServicesComponent } from './possible-services.component';

describe('PossibleServicesComponent', () => {
  let component: PossibleServicesComponent;
  let fixture: ComponentFixture<PossibleServicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PossibleServicesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PossibleServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
