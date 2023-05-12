import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreTemplateComponent } from './core-template.component';

describe('CoreTemplateComponent', () => {
  let component: CoreTemplateComponent;
  let fixture: ComponentFixture<CoreTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreTemplateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoreTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
