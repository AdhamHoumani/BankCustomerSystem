import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdentityTemplateComponent } from './identity-template.component';

describe('IdentityTemplateComponent', () => {
  let component: IdentityTemplateComponent;
  let fixture: ComponentFixture<IdentityTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IdentityTemplateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IdentityTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
