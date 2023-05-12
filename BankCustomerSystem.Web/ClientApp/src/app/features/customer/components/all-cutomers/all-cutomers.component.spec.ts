import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllCutomersComponent } from './all-cutomers.component';

describe('AllCutomersComponent', () => {
  let component: AllCutomersComponent;
  let fixture: ComponentFixture<AllCutomersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllCutomersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllCutomersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
